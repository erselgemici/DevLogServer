using AutoMapper;
using DevLog.Application.Features.Categories.Commands;
using DevLog.Application.Features.Categories.Result;
using DevLog.Application.Localization;
using DevLog.Domain.Entities;
using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Features.Categories.Mappings
{
    // AutoMapper profili, kategori adı isteğin diline göre geliyor
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, GetCategoryQueryResult>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.GetCategoryName(LanguageContext.Current)))
                .ForMember(d => d.CategoryNameEn, o => o.MapFrom(s => s.CategoryNameOf(Languages.English)));

            CreateMap<Category, GetCategoryByIdQueryResult>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.GetCategoryName(LanguageContext.Current)))
                .ForMember(d => d.CategoryNameEn, o => o.MapFrom(s => s.CategoryNameOf(Languages.English)));

            // blogun içindeki kategori bilgisi (sadece id ve ad)
            // CategoryName entity'de yok, dile göre metotla geliyor, o yüzden MapFrom yazmam gerekti
            CreateMap<Category, CategorySummaryResult>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.GetCategoryName(LanguageContext.Current)));

            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<UpdateCategoryCommand, Category>();
        }
    }
}
