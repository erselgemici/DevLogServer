using System.Text.Json.Serialization;
using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Comments.Commands;

// yorum eklerken client'tan gelen veriler (command)
public record CreateCommentCommand : IRequest<BaseResult<object>>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Body { get; init; }
    [JsonIgnore]
    public DateTime CommentDate { get; init; } = DateTime.Now;
    public Guid BlogId { get; init; }
}
