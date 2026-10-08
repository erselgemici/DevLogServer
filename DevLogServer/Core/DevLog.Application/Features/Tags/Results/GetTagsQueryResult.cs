namespace DevLog.Application.Features.Tags.Result;

// etiket listesi, ana sayfadaki "Popüler konular" ve admin blog formunda kullanılıyor
public class GetTagsQueryResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    // bu etikette kaç yazı var (en çok yazısı olan konular önde gösteriliyor)
    public int BlogCount { get; set; }
}
