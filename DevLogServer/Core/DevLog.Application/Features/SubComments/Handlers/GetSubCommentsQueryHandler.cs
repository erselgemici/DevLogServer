using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.SubComments.Queries;
using DevLog.Application.Features.SubComments.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.SubComments.Handlers;

// GetSubCommentsQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetSubCommentsQueryHandler(IRepository<SubComment> repository, IMapper mapper)
    : IRequestHandler<GetSubCommentsQuery, BaseResult<List<GetSubCommentsQueryResult>>>
{
    public Task<BaseResult<List<GetSubCommentsQueryResult>>> Handle(GetSubCommentsQuery request, CancellationToken cancellationToken)
    {
        // yanıt -> yorumu -> yorumun blogu, iç içe Include (nokta ile)
        var values = repository.GetQueryWithIncludes("Comment", "Comment.Blog", "Comment.Blog.Translations")
            .ToList();
        // entity listesini dto listesine çeviriyorum
        var subComments = mapper.Map<List<GetSubCommentsQueryResult>>(values);
        return Task.FromResult(BaseResult<List<GetSubCommentsQueryResult>>.Success(subComments));
    }
}
