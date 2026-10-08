using DevLog.Application.Base;
using DevLog.Application.Features.Categories.Result;
using DevLog.Application.Features.Users.Result;

namespace DevLog.Application.Features.Blogs.Result;

// client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
public class GetBlogsByCategoryIdQueryResult : BaseDto
{
    public string Title { get; set; }
    public string CoverImage { get; set; }
    public string BlogImage { get; set; }
    // listede yazının tamamını göndermiyorum, sadece kısa özet (mapping profilinde dolduruluyor)
    // yazının tamamı sadece detay sayfasında (GetBlogByIdQueryResult) var
    public string Summary { get; set; }
    // kaç dakikalık okuma (TextHelper.ReadingMinutes ile hesaplanıyor)
    public int ReadingMinutes { get; set; }
    public Guid CategoryId { get; set; }
    public CategorySummaryResult Category { get; set; }
    public string UserId { get; set; }
    public GetUsersQueryResult User { get; set; }
}
