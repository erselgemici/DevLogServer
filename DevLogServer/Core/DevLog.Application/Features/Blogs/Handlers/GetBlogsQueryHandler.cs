using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Queries;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Blogs.Handlers;

// GetBlogsQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetBlogsQueryHandler(IRepository<Blog> repository, IMapper mapper)
    : IRequestHandler<GetBlogsQuery, BaseResult<List<GetBlogsQueryResult>>>
{
    public Task<BaseResult<List<GetBlogsQueryResult>>> Handle(GetBlogsQuery request, CancellationToken cancellationToken)
    {
        // Include ile yazar, kategori ve çeviriler tek seferde geliyor (N+1 olmasın diye)
        // NOT: burada await yok çünkü ToList() senkron, o yüzden metot async değil
        // ve sonucu Task.FromResult ile Task'a sarıp dönüyorum (Latest5 handler'ındaki gibi)
        // admin panelde de en yeni yazı en üstte görünsün
        var values = repository.GetQueryWithIncludes("Translations", "User", "Category", "Category.Translations")
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
        // entity listesini dto listesine çeviriyorum
        var blogs = mapper.Map<List<GetBlogsQueryResult>>(values);
        return Task.FromResult(BaseResult<List<GetBlogsQueryResult>>.Success(blogs));
    }
}
