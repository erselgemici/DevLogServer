using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Messages.Commands;

// silme işlemi için sadece Id yetiyor
public record RemoveMessageCommand(Guid Id) : IRequest<BaseResult<object>>;
