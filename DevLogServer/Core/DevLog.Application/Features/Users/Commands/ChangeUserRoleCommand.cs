using System.Text.Json.Serialization;
using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Users.Commands;

// admin'in bir kullanıcının rolünü değiştirmesi (Admin / Writer)
public class ChangeUserRoleCommand : IRequest<BaseResult<object>>
{
    public string UserId { get; set; }
    public string Role { get; set; }

    // endpoint token'dan dolduruyor, admin kendi yetkisini kaldırmasın diye
    [JsonIgnore]
    public string? CurrentUserId { get; set; }
}
