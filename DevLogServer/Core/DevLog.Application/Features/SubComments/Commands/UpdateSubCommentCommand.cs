using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.SubComments.Commands;

// alt yorum güncellemek için gelen model, Id ile hangi kayıt olduğunu buluyoruz
public record UpdateSubCommentCommand : IRequest<BaseResult<object>>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Body { get; init; }
    public Guid CommentId { get; init; }
}
