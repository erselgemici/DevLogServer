using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Comments.Queries;
using DevLog.Application.Features.Comments.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Comments.Handlers;

// GetCommentByIdQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetCommentByIdQueryHandler(IRepository<Comment> repository, IMapper mapper)
    : IRequestHandler<GetCommentByIdQuery, BaseResult<GetCommentByIdQueryResult>>
{
    public Task<BaseResult<GetCommentByIdQueryResult>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        // blog başlığı da lazım olduğu için Include'lu sorgu (FindAsync Include desteklemiyor)
        var value = repository.GetQueryWithIncludes("Blog", "Blog.Translations", "SubComments")
            .FirstOrDefault(x => x.Id == request.Id);
        // kayıt yoksa hata dönüyorum
        if (value is null)
        {
            return Task.FromResult(BaseResult<GetCommentByIdQueryResult>.NotFound("Yorum bulunamadı"));
        }

        // entity'i dto'ya çeviriyorum
        var comment = mapper.Map<GetCommentByIdQueryResult>(value);
        return Task.FromResult(BaseResult<GetCommentByIdQueryResult>.Success(comment));
    }
}
