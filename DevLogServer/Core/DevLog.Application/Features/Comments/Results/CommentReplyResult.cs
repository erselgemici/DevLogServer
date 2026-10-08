namespace DevLog.Application.Features.Comments.Result;

// yoruma verilen cevap (SubComment), blog detayında yorumun altında gösteriliyor
// NOT: GetSubCommentsQueryResult'ı kullanmadım çünkü onun içinde Comment var,
// yorumun içinde cevap, cevabın içinde yine yorum... döngü olurdu. sadece gereken alanlar
public class CommentReplyResult
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Body { get; set; }
    public DateTime CommentDate { get; set; }
}
