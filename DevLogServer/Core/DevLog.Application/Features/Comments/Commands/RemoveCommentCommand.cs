using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Comments.Commands;

// silme işlemi için sadece Id yetiyor
public record RemoveCommentCommand(Guid Id) : IRequest<BaseResult<object>>;
