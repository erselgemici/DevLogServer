using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Blogs.Result;

namespace DevLog.Application.Features.Blogs.Queries;

// bir etiketteki (konudaki) bütün yazılar, slug ile: /api/blogs/byTag/dotnet
public record GetBlogsByTagQuery(string Slug) : IRequest<BaseResult<List<GetBlogsQueryResult>>>;
