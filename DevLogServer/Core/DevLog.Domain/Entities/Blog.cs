using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities;

// blog yazısı tablosu, Title ve Description varsayılan dil (Türkçe)
public class Blog : BaseEntity
{
    public string Title { get; set; }
    public string CoverImage { get; set; }
    public string BlogImage { get; set; }
    public string Description { get; set; }
    // her blog bir kategoriye bağlı
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }
    // blogu yazan kullanıcı
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }
    // virtual olmasının sebebi lazy loading
    public virtual IList<Comment> Comments { get; set; }
    // diğer dillerdeki başlık ve açıklamalar
    public virtual IList<BlogTranslation> Translations { get; set; }
    // blogun etiketleri (çoka çok), Tag tarafında da Blogs listesi var
    public virtual IList<Tag> Tags { get; set; }

    // istenen dilde çeviri varsa onu, yoksa Türkçe başlığı dönüyor
    public string GetTitle(string language) => TitleOf(language) ?? Title;

    public string GetDescription(string language) => DescriptionOf(language) ?? Description;

    // sadece çeviriyi dönüyor, yoksa null (admin panelde İngilizce alanı doldurmak için)
    public string? TitleOf(string language)
    {
        if (language == Languages.Default) return null;
        var value = Translations?.FirstOrDefault(x => x.LanguageCode == language)?.Title;
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public string? DescriptionOf(string language)
    {
        if (language == Languages.Default) return null;
        var value = Translations?.FirstOrDefault(x => x.LanguageCode == language)?.Description;
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    // çeviriyi ekliyor/güncelliyor, iki alan da boşsa çeviriyi siliyor
    public void SetTranslation(string language, string? title, string? description)
    {
        Translations ??= new List<BlogTranslation>();
        var existing = Translations.FirstOrDefault(x => x.LanguageCode == language);

        if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(description))
        {
            if (existing is not null) Translations.Remove(existing);
            return;
        }

        if (existing is null)
        {
            existing = new BlogTranslation { LanguageCode = language };
            Translations.Add(existing);
        }

        existing.Title = title ?? string.Empty;
        existing.Description = description ?? string.Empty;
    }
}
