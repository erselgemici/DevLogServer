using AutoMapper;
using DevLog.Application.Features.Blogs.Commands;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Application.Helpers;
using DevLog.Application.Localization;
using DevLog.Domain.Entities;
using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Features.Blogs.Mappings;

// AutoMapper profili, blog entity'si ile dto/command arasındaki eşleşmeler
// Title ve Description isteğin diline göre geliyor (çeviri yoksa Türkçe)
public class BlogMappingProfile : Profile
{
    public BlogMappingProfile()
    {
        // liste dto'ları: başlık isteğin dilinde, içerik yerine kısa özet
        // MapFrom içinde kendi metodumu çağırabiliyorum (TextHelper.Summarize)
        // NOT: Expression içinde opsiyonel parametre kullanılamıyor, o yüzden 220'yi açıkça yazdım
        CreateMap<Blog, GetBlogsQueryResult>()
            .ForMember(d => d.Title, o => o.MapFrom(s => s.GetTitle(LanguageContext.Current)))
            .ForMember(d => d.Summary, o => o.MapFrom(s => TextHelper.Summarize(s.GetDescription(LanguageContext.Current), 220)))
            .ForMember(d => d.ReadingMinutes, o => o.MapFrom(s => TextHelper.ReadingMinutes(s.GetDescription(LanguageContext.Current))));

        CreateMap<Blog, GetLatest5BlogsQueryResult>()
            .ForMember(d => d.Title, o => o.MapFrom(s => s.GetTitle(LanguageContext.Current)))
            .ForMember(d => d.Summary, o => o.MapFrom(s => TextHelper.Summarize(s.GetDescription(LanguageContext.Current), 220)))
            .ForMember(d => d.ReadingMinutes, o => o.MapFrom(s => TextHelper.ReadingMinutes(s.GetDescription(LanguageContext.Current))));

        CreateMap<Blog, GetBlogsByCategoryIdQueryResult>()
            .ForMember(d => d.Title, o => o.MapFrom(s => s.GetTitle(LanguageContext.Current)))
            .ForMember(d => d.Summary, o => o.MapFrom(s => TextHelper.Summarize(s.GetDescription(LanguageContext.Current), 220)))
            .ForMember(d => d.ReadingMinutes, o => o.MapFrom(s => TextHelper.ReadingMinutes(s.GetDescription(LanguageContext.Current))));

        // detay sayfası: içeriğin tamamı + admin panel için İngilizce alanlar
        CreateMap<Blog, GetBlogByIdQueryResult>()
            .ForMember(d => d.Title, o => o.MapFrom(s => s.GetTitle(LanguageContext.Current)))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.GetDescription(LanguageContext.Current)))
            .ForMember(d => d.TitleEn, o => o.MapFrom(s => s.TitleOf(Languages.English)))
            .ForMember(d => d.DescriptionEn, o => o.MapFrom(s => s.DescriptionOf(Languages.English)))
            .ForMember(d => d.ReadingMinutes, o => o.MapFrom(s => TextHelper.ReadingMinutes(s.GetDescription(LanguageContext.Current))));

        // yorumların içindeki blog bilgisi (BlogSummaryResult), Id'yi AutoMapper isimden kendisi eşliyor
        CreateMap<Blog, BlogSummaryResult>()
            .ForMember(d => d.Title, o => o.MapFrom(s => s.GetTitle(LanguageContext.Current)));

        // command -> entity, İngilizce alanları handler'da ayrıca çeviri olarak kaydediyorum
        CreateMap<CreateBlogCommand, Blog>();
        CreateMap<UpdateBlogCommand, Blog>();
    }
}
