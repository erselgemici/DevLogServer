using DevLog.Domain.Entities.Common;
namespace DevLog.Domain.Entities;

// kategori tablosu, bir kategoride birden fazla blog olabilir
public class Category : BaseEntity
{
    public string CategoryName { get; set; }
    public virtual IList<Blog> Blogs { get; set; }
    // kategori adının diğer dillerdeki karşılıkları
    public virtual IList<CategoryTranslation> Translations { get; set; }

    public string GetCategoryName(string language) => CategoryNameOf(language) ?? CategoryName;

    public string? CategoryNameOf(string language)
    {
        if (language == Languages.Default) return null;
        var value = Translations?.FirstOrDefault(x => x.LanguageCode == language)?.CategoryName;
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public void SetTranslation(string language, string? categoryName)
    {
        Translations ??= new List<CategoryTranslation>();
        var existing = Translations.FirstOrDefault(x => x.LanguageCode == language);

        if (string.IsNullOrWhiteSpace(categoryName))
        {
            if (existing is not null) Translations.Remove(existing);
            return;
        }

        if (existing is null)
        {
            existing = new CategoryTranslation { LanguageCode = language };
            Translations.Add(existing);
        }

        existing.CategoryName = categoryName;
    }
}
