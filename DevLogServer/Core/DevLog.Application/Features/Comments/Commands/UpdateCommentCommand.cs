using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Comments.Commands;

// yorum güncellemek için gelen model, Id ile hangi kayıt olduğunu buluyoruz
public record UpdateCommentCommand : IRequest<BaseResult<object>>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Body { get; init; }
    public Guid BlogId { get; init; }
}
