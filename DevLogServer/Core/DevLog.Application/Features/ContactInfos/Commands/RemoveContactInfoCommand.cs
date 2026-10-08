using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.ContactInfos.Commands;

// silme işlemi için sadece Id yetiyor
public record RemoveContactInfoCommand(Guid Id) : IRequest<BaseResult<object>>;
