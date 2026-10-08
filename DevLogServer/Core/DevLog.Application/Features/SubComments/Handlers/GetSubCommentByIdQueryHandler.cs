using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.SubComments.Queries;
using DevLog.Application.Features.SubComments.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.SubComments.Handlers;

// GetSubCommentByIdQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetSubCommentByIdQueryHandler(IRepository<SubComment> repository, IMapper mapper)
    : IRequestHandler<GetSubCommentByIdQuery, BaseResult<GetSubCommentByIdQueryResult>>
{
    public Task<BaseResult<GetSubCommentByIdQueryResult>> Handle(GetSubCommentByIdQuery request, CancellationToken cancellationToken)
    {
        // yanıt -> yorumu -> yorumun blogu, iç içe Include (nokta ile)
        var value = repository.GetQueryWithIncludes("Comment", "Comment.Blog", "Comment.Blog.Translations")
            .FirstOrDefault(x => x.Id == request.Id);
        // kayıt yoksa hata dönüyorum
        if (value is null)
        {
            return Task.FromResult(BaseResult<GetSubCommentByIdQueryResult>.NotFound("Yanıt bulunamadı"));
        }

        // entity'i dto'ya çeviriyorum
        var subComment = mapper.Map<GetSubCommentByIdQueryResult>(value);
        return Task.FromResult(BaseResult<GetSubCommentByIdQueryResult>.Success(subComment));
    }
}
