using MediatR;
using Microsoft.AspNetCore.Identity;
using DevLog.Application.Base;
using DevLog.Application.Constants;
using DevLog.Application.Features.Users.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Users.Handlers;

// CreateUserCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class CreateUserCommandHandler(UserManager<AppUser> userManager)
    : IRequestHandler<CreateUserCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName
        };

        // context'e ekliyor ama daha veritabanına yazmadı
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BaseResult<object>.Fail(result.Errors);
        }

        // yeni kayıt olan herkes yazar rolüyle başlıyor, admin yetkisini sadece admin verebilir
        await userManager.AddToRoleAsync(user, Roles.Writer);

        return BaseResult<object>.Success("Kullanıcı başarıyla oluşturuldu");
    }
}
