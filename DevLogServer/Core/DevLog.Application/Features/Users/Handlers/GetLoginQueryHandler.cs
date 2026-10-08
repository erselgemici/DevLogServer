using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Users.Queries;
using DevLog.Application.Features.Users.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Users.Handlers;

// GetLoginQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetLoginQueryHandler(UserManager<AppUser> userManager, IJwtService jwtService, IMapper mapper)
    : IRequestHandler<GetLoginQuery, BaseResult<GetLoginQueryResult>>
{
    public async Task<BaseResult<GetLoginQueryResult>> Handle(GetLoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return BaseResult<GetLoginQueryResult>.Fail("E-posta veya şifre hatalı");
        }

        // entity'i dto'ya çeviriyorum
        var userResult = mapper.Map<GetUsersQueryResult>(user);

        var response = await jwtService.GenerateTokenAsync(userResult);
        // kayıt yoksa hata dönüyorum
        if (response is null)
        {
            return BaseResult<GetLoginQueryResult>.Fail("Token oluşturulamadı");
        }

        return BaseResult<GetLoginQueryResult>.Success(response);
    }
}
