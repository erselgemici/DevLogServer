using System.Text.Json.Serialization;
using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Messages.Commands;

// mesaj eklerken client'tan gelen veriler (command)
public record CreateMessageCommand : IRequest<BaseResult<object>>
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Subject { get; init; }
    public string MessageBody { get; init; }
    [JsonIgnore]
    public bool IsRead { get; init; } = false;
}
