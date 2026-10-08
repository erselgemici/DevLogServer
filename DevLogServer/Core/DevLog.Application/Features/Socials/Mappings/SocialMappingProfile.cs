using AutoMapper;
using DevLog.Application.Features.Socials.Commands;
using DevLog.Application.Features.Socials.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Socials.Mappings;

// AutoMapper profili, sosyal medya linki entity'si ile dto/command arasındaki eşleşmeler
public class SocialMappingProfile : Profile
{
    public SocialMappingProfile()
    {
        // ReverseMap sayesinde iki yönlü de çalışıyor
        CreateMap<Social, GetSocialsQueryResult>().ReverseMap();
        CreateMap<Social, GetSocialByIdQueryResult>().ReverseMap();
        CreateMap<Social, CreateSocialCommand>().ReverseMap();
        CreateMap<Social, UpdateSocialCommand>().ReverseMap();
    }
}
