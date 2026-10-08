using DevLog.Application.Base;
using DevLog.Application.Features.Blogs.Result;

namespace DevLog.Application.Features.Comments.Result;

// client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
public class GetCommentsQueryResult : BaseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Body { get; set; }
    public DateTime CommentDate { get; set; }
    public Guid BlogId { get; set; }
    public BlogSummaryResult Blog { get; set; }
    // yoruma gelen cevaplar (yazarın cevabı vs)
    public IList<CommentReplyResult> SubComments { get; set; }
}
