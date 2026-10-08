using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Queries;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Blogs.Handlers;

// GetBlogsByCategoryIdQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetBlogsByCategoryIdQueryHandler(IRepository<Blog> repository, IMapper mapper)
    : IRequestHandler<GetBlogsByCategoryIdQuery, BaseResult<List<GetBlogsByCategoryIdQueryResult>>>
{
    public Task<BaseResult<List<GetBlogsByCategoryIdQueryResult>>> Handle(GetBlogsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        // sadece bu kategorinin blogları, yazar ve çevirileriyle beraber
        // Where'i ToList'ten önce yazdığım için filtre SQL'de çalışıyor (bütün tabloyu çekmiyor)
        var values = repository.GetQueryWithIncludes("Translations", "User", "Category", "Category.Translations")
            .Where(x => x.CategoryId == request.CategoryId)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
        // entity listesini dto listesine çeviriyorum
        var blogs = mapper.Map<List<GetBlogsByCategoryIdQueryResult>>(values);
        return Task.FromResult(BaseResult<List<GetBlogsByCategoryIdQueryResult>>.Success(blogs));
    }
}
