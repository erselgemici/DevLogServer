namespace DevLog.Application.Features.Categories.Result;

// blogun içinde sadece kategori adını gösteriyorum, o yüzden id + ad yeterli
// NOT: blog -> kategori -> kategorinin blogları -> ... diye döngü oluşuyordu (circular reference)
// her dto'ya ihtiyacı kadar alan koymak hem döngüyü bitiriyor hem cevabı küçültüyor
public class CategorySummaryResult
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
}
