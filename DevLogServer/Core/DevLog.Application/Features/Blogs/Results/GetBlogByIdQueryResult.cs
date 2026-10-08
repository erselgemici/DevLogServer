using DevLog.Application.Base;
using DevLog.Application.Features.Categories.Result;
using DevLog.Application.Features.Comments.Result;
using DevLog.Application.Features.Tags.Result;
using DevLog.Application.Features.Users.Result;

namespace DevLog.Application.Features.Blogs.Result;

// client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
public class GetBlogByIdQueryResult : BaseDto
{
    public string Title { get; set; }
    public string CoverImage { get; set; }
    public string BlogImage { get; set; }
    public string Description { get; set; }
    // admin panelde İngilizce alanları doldurmak için
    public string? TitleEn { get; set; }
    public string? DescriptionEn { get; set; }
    public Guid CategoryId { get; set; }
    public CategorySummaryResult Category { get; set; }
    public string UserId { get; set; }
    public GetUsersQueryResult User { get; set; }
    public IList<GetCommentsQueryResult> Comments { get; set; }
    public int ReadingMinutes { get; set; }
    // yazının konuları, detay sayfasında etiket olarak gösteriliyor
    public IList<TagSummaryResult> Tags { get; set; }
}
