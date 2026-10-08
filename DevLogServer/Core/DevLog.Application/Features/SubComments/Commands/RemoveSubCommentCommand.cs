using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.SubComments.Commands;

// silme işlemi için sadece Id yetiyor
public record RemoveSubCommentCommand(Guid Id) : IRequest<BaseResult<object>>;
