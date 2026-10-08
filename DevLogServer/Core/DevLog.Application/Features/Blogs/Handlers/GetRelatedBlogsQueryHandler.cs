using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Queries;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Blogs.Handlers;

// GetRelatedBlogsQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetRelatedBlogsQueryHandler(IRepository<Blog> repository, IMapper mapper)
    : IRequestHandler<GetRelatedBlogsQuery, BaseResult<List<GetBlogsQueryResult>>>
{
    public Task<BaseResult<List<GetBlogsQueryResult>>> Handle(GetRelatedBlogsQuery request, CancellationToken cancellationToken)
    {
        // 1) önce bu yazının etiketlerinin id'lerini alıyorum
        //    SelectMany: her blogun etiket listesini tek bir listeye düzleştiriyor
        var tagIds = repository.GetQuery()
            .Where(x => x.Id == request.BlogId)
            .SelectMany(x => x.Tags.Select(t => t.Id))
            .ToList();

        // 2) aynı etiketlerden en az birine sahip diğer yazılar
        //    en çok ortak etiketi olan en üstte, eşitse yeni olan önce
        //    NOT: tagIds.Contains(...) SQL'de IN (...) olarak çalışıyor
        var values = repository.GetQueryWithIncludes("Translations", "User", "Category", "Category.Translations")
            .Where(x => x.Id != request.BlogId && x.Tags.Any(t => tagIds.Contains(t.Id)))
            .OrderByDescending(x => x.Tags.Count(t => tagIds.Contains(t.Id)))
            .ThenByDescending(x => x.CreatedAt)
            .Take(4)
            .ToList();

        var blogs = mapper.Map<List<GetBlogsQueryResult>>(values);
        return Task.FromResult(BaseResult<List<GetBlogsQueryResult>>.Success(blogs));
    }
}
