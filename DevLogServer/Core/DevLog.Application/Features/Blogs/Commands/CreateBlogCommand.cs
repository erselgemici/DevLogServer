using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Blogs.Commands;

// blog eklerken client'tan gelen veriler (command)
public class CreateBlogCommand : IRequest<BaseResult<object>>
{
    public string Title { get; set; }
    public string CoverImage { get; set; }
    public string BlogImage { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public string UserId { get; set; }
    // İngilizce karşılıklar (opsiyonel)
    public string? TitleEn { get; set; }
    public string? DescriptionEn { get; set; }
    // seçilen etiketlerin id'leri (admin paneldeki checkbox'lardan geliyor)
    public List<Guid>? TagIds { get; set; }
}
