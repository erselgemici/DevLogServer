using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Messages.Commands;

// mesaj güncellemek için gelen model, Id ile hangi kayıt olduğunu buluyoruz
public record UpdateMessageCommand : IRequest<BaseResult<object>>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string Subject { get; init; }
    public string MessageBody { get; init; }
    public bool IsRead { get; init; }
}
