using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Features.Users.Result;

namespace DevLog.Application.Features.Users.Queries;

// giriş için email ve şifre burada geliyor
public class GetLoginQuery : IRequest<BaseResult<GetLoginQueryResult>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
