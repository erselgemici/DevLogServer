using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Queries;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Blogs.Handlers;

// GetLatest5BlogsQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetLatest5BlogsQueryHandler(IRepository<Blog> repository, IMapper mapper)
    : IRequestHandler<GetLatest5BlogsQuery, BaseResult<List<GetLatest5BlogsQueryResult>>>
{
    public Task<BaseResult<List<GetLatest5BlogsQueryResult>>> Handle(GetLatest5BlogsQuery request, CancellationToken cancellationToken)
    {
        // Take(5) ToList'ten önce olduğu için SQL'de TOP(5) olarak çalışıyor
        var blogs = repository.GetQueryWithIncludes("Translations", "User", "Category", "Category.Translations")
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .ToList();

        // entity listesini dto listesine çeviriyorum
        var mappedBlogs = mapper.Map<List<GetLatest5BlogsQueryResult>>(blogs);
        return Task.FromResult(BaseResult<List<GetLatest5BlogsQueryResult>>.Success(mappedBlogs));
    }
}
