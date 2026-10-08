using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Blogs.Commands;

// silme işlemi için Id yetiyor, kullanıcı bilgisi yetki kontrolü için endpoint'ten geliyor
public record RemoveBlogCommand(Guid Id, string? CurrentUserId = null, bool IsAdmin = false) : IRequest<BaseResult<object>>;
