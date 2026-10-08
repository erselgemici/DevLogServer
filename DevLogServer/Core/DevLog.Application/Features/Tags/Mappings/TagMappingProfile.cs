using AutoMapper;
using DevLog.Application.Features.Tags.Result;
using DevLog.Application.Localization;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Tags.Mappings;

// blog detayındaki etiketler için, ad isteğin diline göre geliyor
public class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<Tag, TagSummaryResult>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.GetName(LanguageContext.Current)));
    }
}
