using DevLog.Application.Features.Users.Result;

namespace DevLog.Application.Contracts.Persistence;

// token üretme işi, implementasyonu Persistence katmanında
public interface IJwtService
{
    Task<GetLoginQueryResult> GenerateTokenAsync(GetUsersQueryResult result);
}
