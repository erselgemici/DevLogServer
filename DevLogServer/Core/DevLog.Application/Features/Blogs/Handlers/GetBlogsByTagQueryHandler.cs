using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Queries;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Blogs.Handlers;

// GetBlogsByTagQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetBlogsByTagQueryHandler(IRepository<Blog> repository, IMapper mapper)
    : IRequestHandler<GetBlogsByTagQuery, BaseResult<List<GetBlogsQueryResult>>>
{
    public Task<BaseResult<List<GetBlogsQueryResult>>> Handle(GetBlogsByTagQuery request, CancellationToken cancellationToken)
    {
        // Tags.Any(...) -> etiketlerinden en az biri bu slug olan bloglar
        // çoka çok ilişkide filtreleme böyle yazılıyor, EF ara tabloyla (BlogTags) JOIN'liyor
        var values = repository.GetQueryWithIncludes("Translations", "User", "Category", "Category.Translations")
            .Where(x => x.Tags.Any(t => t.Slug == request.Slug))
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        var blogs = mapper.Map<List<GetBlogsQueryResult>>(values);
        return Task.FromResult(BaseResult<List<GetBlogsQueryResult>>.Success(blogs));
    }
}
