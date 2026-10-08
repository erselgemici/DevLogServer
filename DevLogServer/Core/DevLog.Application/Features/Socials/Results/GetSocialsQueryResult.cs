using DevLog.Application.Base;

namespace DevLog.Application.Features.Socials.Result;

// client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
public class GetSocialsQueryResult : BaseDto
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string Icon { get; set; }
}
