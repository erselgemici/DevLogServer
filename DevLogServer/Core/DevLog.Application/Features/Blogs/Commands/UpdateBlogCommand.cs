using System.Text.Json.Serialization;
using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Blogs.Commands;

// blog güncellemek için gelen model, Id ile hangi kayıt olduğunu buluyoruz
public class UpdateBlogCommand : IRequest<BaseResult<object>>
{
    public Guid Id { get; set; }
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

    // client'tan gelmiyor, endpoint token'dan dolduruyor (yetki kontrolü için)
    [JsonIgnore]
    public string? CurrentUserId { get; set; }
    [JsonIgnore]
    public bool IsAdmin { get; set; }
}
