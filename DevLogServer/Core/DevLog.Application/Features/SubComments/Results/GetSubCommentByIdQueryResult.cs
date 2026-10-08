using DevLog.Application.Base;
using DevLog.Application.Features.Comments.Result;

namespace DevLog.Application.Features.SubComments.Result;

// client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
public class GetSubCommentByIdQueryResult : BaseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Body { get; set; }
    public DateTime CommentDate { get; set; }
    public Guid CommentId { get; set; }
    public GetCommentsQueryResult Comment { get; set; }
}
