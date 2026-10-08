using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Comments.Queries;
using DevLog.Application.Features.Comments.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Comments.Handlers;

// GetCommentsQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetCommentsQueryHandler(IRepository<Comment> repository, IMapper mapper)
    : IRequestHandler<GetCommentsQuery, BaseResult<List<GetCommentsQueryResult>>>
{
    public Task<BaseResult<List<GetCommentsQueryResult>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        // admin yorum listesinde blog başlığı da görünüyor, Blog'u ve çevirisini de Include ediyorum
        // en yeni yorum en üstte
        var values = repository.GetQueryWithIncludes("Blog", "Blog.Translations", "SubComments")
            .OrderByDescending(x => x.CommentDate)
            .ToList();
        // entity listesini dto listesine çeviriyorum
        var comments = mapper.Map<List<GetCommentsQueryResult>>(values);
        return Task.FromResult(BaseResult<List<GetCommentsQueryResult>>.Success(comments));
    }
}
