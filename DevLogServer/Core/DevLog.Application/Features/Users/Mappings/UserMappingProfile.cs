using AutoMapper;
using DevLog.Application.Features.Users.Commands;
using DevLog.Application.Features.Users.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Users.Mappings;

// AutoMapper profili, kullanıcı entity'si ile dto/command arasındaki eşleşmeler
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // ReverseMap sayesinde iki yönlü de çalışıyor
        CreateMap<AppUser, CreateUserCommand>().ReverseMap();
        CreateMap<AppUser, GetUsersQueryResult>().ReverseMap();
    }
}
