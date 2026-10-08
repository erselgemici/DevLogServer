using System.Text.Json.Serialization;
using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.SubComments.Commands;

// alt yorum eklerken client'tan gelen veriler (command)
public record CreateSubCommentCommand : IRequest<BaseResult<object>>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Body { get; init; }
    [JsonIgnore]
    public DateTime CommentDate { get; init; } = DateTime.Now;
    public Guid CommentId { get; init; }
}
