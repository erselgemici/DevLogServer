using MediatR;
using Microsoft.AspNetCore.Identity;
using DevLog.Application.Base;
using DevLog.Application.Constants;
using DevLog.Application.Features.Users.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Users.Handlers;

// ChangeUserRoleCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class ChangeUserRoleCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<ChangeUserRoleCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return BaseResult<object>.NotFound("Kullanıcı bulunamadı");
        }

        // admin kendi admin yetkisini kaldırırsa panele kimse giremeyebilir
        if (user.Id == request.CurrentUserId && request.Role != Roles.Admin)
        {
            return BaseResult<object>.Fail("Kendi admin yetkinizi kaldıramazsınız");
        }

        // önce eski rolleri kaldırıp yenisini ekliyorum (kullanıcının tek rolü olsun)
        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await userManager.AddToRoleAsync(user, request.Role);

        return result.Succeeded
            ? BaseResult<object>.Success("Rol güncellendi")
            : BaseResult<object>.Fail(result.Errors);
    }
}
