using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Users.Result;
using DevLog.Application.Options;
using DevLog.Domain.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DevLog.Persistence.Concrete
{
    // login olan kullanıcıya jwt token üretiyor
    public class JwtService(UserManager<AppUser> _userManager, IOptions<JwtTokenOptions> tokenOptions) : IJwtService
    {
        private readonly JwtTokenOptions _jwtTokenOptions = tokenOptions.Value;

        public async Task<GetLoginQueryResult> GenerateTokenAsync(GetUsersQueryResult result)
        {
            var user = await _userManager.FindByNameAsync(result.UserName);
            if (user is null) return null;

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_jwtTokenOptions.Key));
            var dateTimeNow = DateTime.UtcNow;
            var expires = dateTimeNow.AddMinutes(_jwtTokenOptions.ExpireInMinutes);

            // token'ın içine koyduğum bilgiler, angular tarafında decode edip kullanıyorum
            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Name, user.UserName!),
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new("fullName", string.Join(" ", user.FirstName, user.LastName)),
                // admin panelde profil resmini göstermek için
                new("imageUrl", user.ImageUrl ?? string.Empty),
            ];

            // kullanıcının rolleri varsa token'a ekliyorum
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim("role", role)));

            // token'ı oluşturup key ile imzalıyorum
            JwtSecurityToken jwtSecurityToken = new(
                issuer: _jwtTokenOptions.Issuer,
                audience: _jwtTokenOptions.Audience,
                claims: claims,
                notBefore: dateTimeNow,
                expires: expires,
                signingCredentials: new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            return new GetLoginQueryResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpirationTime = expires
            };
        }
    }
}
