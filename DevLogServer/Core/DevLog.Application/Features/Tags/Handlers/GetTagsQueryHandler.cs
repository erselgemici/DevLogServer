using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Tags.Queries;
using DevLog.Application.Features.Tags.Result;
using DevLog.Application.Localization;
using DevLog.Domain.Entities;
using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Features.Tags.Handlers;

// GetTagsQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetTagsQueryHandler(IRepository<Tag> repository)
    : IRequestHandler<GetTagsQuery, BaseResult<List<GetTagsQueryResult>>>
{
    public Task<BaseResult<List<GetTagsQueryResult>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        // NOT: burada AutoMapper kullanmadım, Select ile direkt dto'ya çevirdim (projection).
        // x.Blogs.Count SQL'de COUNT olarak çalışıyor, bloglar belleğe hiç yüklenmiyor.
        // dil seçimi (GetName) SQL'e çevrilemediği için önce gerekli alanları alıp sonra bellekte seçiyorum
        var language = LanguageContext.Current;

        var tags = repository.GetQuery()
            .Select(x => new { x.Id, x.Name, x.NameEn, x.Slug, BlogCount = x.Blogs.Count })
            .ToList()
            .Select(x => new GetTagsQueryResult
            {
                Id = x.Id,
                Slug = x.Slug,
                Name = language == Languages.English && !string.IsNullOrWhiteSpace(x.NameEn) ? x.NameEn : x.Name,
                BlogCount = x.BlogCount
            })
            .OrderByDescending(x => x.BlogCount)
            .ThenBy(x => x.Name)
            .ToList();

        return Task.FromResult(BaseResult<List<GetTagsQueryResult>>.Success(tags));
    }
}
