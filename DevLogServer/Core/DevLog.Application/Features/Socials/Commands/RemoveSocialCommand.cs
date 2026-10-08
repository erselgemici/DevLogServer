using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Socials.Commands;

// silme işlemi için sadece Id yetiyor
public record RemoveSocialCommand(Guid Id) : IRequest<BaseResult<object>>;
