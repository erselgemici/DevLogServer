using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Categories.Commands;

// silme işlemi için sadece Id yetiyor
public record RemoveCategoryCommand(Guid Id) : IRequest<BaseResult<bool>>;