using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Queries;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Blogs.Handlers;

// SearchBlogsQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class SearchBlogsQueryHandler(IRepository<Blog> repository, IMapper mapper)
    : IRequestHandler<SearchBlogsQuery, BaseResult<List<GetBlogsQueryResult>>>
{
    public Task<BaseResult<List<GetBlogsQueryResult>>> Handle(SearchBlogsQuery request, CancellationToken cancellationToken)
    {
        var text = request.Text.Trim();

        // Contains -> SQL'de LIKE '%kelime%' oluyor
        // başlıkta, içerikte, İngilizce çeviride ve etiket adında arıyorum
        // NOT: SQL Server'ın varsayılan collation'ı büyük/küçük harfe duyarsız, ToLower'a gerek yok
        var values = repository.GetQueryWithIncludes("Translations", "User", "Category", "Category.Translations")
            .Where(x => x.Title.Contains(text)
                     || x.Description.Contains(text)
                     || x.Translations.Any(t => t.Title.Contains(text) || t.Description.Contains(text))
                     || x.Tags.Any(t => t.Name.Contains(text)))
            // başlığında geçenler önce gelsin (true > false, SQL'de CASE WHEN'e çevriliyor)
            .OrderByDescending(x => x.Title.Contains(text))
            .ThenByDescending(x => x.CreatedAt)
            .Take(30)
            .ToList();

        var blogs = mapper.Map<List<GetBlogsQueryResult>>(values);
        return Task.FromResult(BaseResult<List<GetBlogsQueryResult>>.Success(blogs));
    }
}
