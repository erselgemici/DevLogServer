using MediatR;
using DevLog.Application.Base;

namespace DevLog.Application.Features.Users.Commands;

// kullanıcı eklerken client'tan gelen veriler (command)
public class CreateUserCommand : IRequest<BaseResult<object>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
