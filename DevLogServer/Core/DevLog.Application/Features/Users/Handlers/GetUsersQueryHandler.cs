using MediatR;
using Microsoft.AspNetCore.Identity;
using DevLog.Application.Base;
using DevLog.Application.Constants;
using DevLog.Application.Features.Users.Queries;
using DevLog.Application.Features.Users.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Users.Handlers;

// GetUsersQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetUsersQueryHandler(UserManager<AppUser> userManager)
    : IRequestHandler<GetUsersQuery, BaseResult<List<GetUserListQueryResult>>>
{
    public async Task<BaseResult<List<GetUserListQueryResult>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // önceden user.Blogs.Count yazıyordum, lazy loading her kullanıcının bütün bloglarını
        // (içerikleriyle!) yüklüyordu, sadece saymak için. Select içinde x.Blogs.Count yazınca
        // EF bunu SQL'de COUNT(*) olarak çalıştırıyor, bloglar hiç yüklenmiyor
        // new { ... } = anonymous type, sadece burada kullanacağım için ayrı class açmadım
        var users = userManager.Users
            .OrderBy(x => x.FirstName)
            .Select(x => new { User = x, BlogCount = x.Blogs.Count })
            .ToList();
        var result = new List<GetUserListQueryResult>();

        foreach (var item in users)
        {
            var user = item.User;
            // her kullanıcının rolünü identity'den okuyorum
            var roles = await userManager.GetRolesAsync(user);

            result.Add(new GetUserListQueryResult
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                UserName = user.UserName!,
                ImageUrl = user.ImageUrl,
                Role = roles.Contains(Roles.Admin) ? Roles.Admin : Roles.Writer,
                BlogCount = item.BlogCount
            });
        }

        return BaseResult<List<GetUserListQueryResult>>.Success(result);
    }
}
