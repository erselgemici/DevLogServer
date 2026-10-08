namespace DevLog.Application.Features.Tags.Result;

// blog detayında etiketleri göstermek için (tıklanınca /tag/slug sayfasına gidiyor)
public class TagSummaryResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
}
