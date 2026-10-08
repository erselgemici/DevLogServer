namespace DevLog.Application.Features.Blogs.Result;

// yorum listesinde sadece "hangi bloga yazılmış" bilgisi lazım, id ve başlık yeterli
// NOT: önceden burada GetBlogsQueryResult vardı, o da kategoriyi, kategori de bütün blogları
// içerdiği için tek yorum için koca bir veri zinciri dönüyordu. küçük dto = küçük json
public class BlogSummaryResult
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}
