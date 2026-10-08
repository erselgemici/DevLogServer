using AutoMapper;
using DevLog.Application.Features.ContactInfos.Commands;
using DevLog.Application.Features.ContactInfos.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.ContactInfos.Mappings;

// AutoMapper profili, iletişim bilgisi entity'si ile dto/command arasındaki eşleşmeler
public class ContactInfoMappingProfile : Profile
{
    public ContactInfoMappingProfile()
    {
        // ReverseMap sayesinde iki yönlü de çalışıyor
        CreateMap<ContactInfo, GetContactInfosQueryResult>().ReverseMap();
        CreateMap<ContactInfo, GetContactInfoByIdQueryResult>().ReverseMap();
        CreateMap<ContactInfo, CreateContactInfoCommand>().ReverseMap();
        CreateMap<ContactInfo, UpdateContactInfoCommand>().ReverseMap();
    }
}
