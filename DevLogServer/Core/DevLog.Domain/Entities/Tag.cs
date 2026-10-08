using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities;

// etiket (konu) tablosu: .NET, Angular, SQL gibi
// kategori ile farkı: bir blogun tek kategorisi var ama birden fazla etiketi olabiliyor
// ör: "EF Core'da İlişkiler" yazısı Backend kategorisinde ama .NET, EF Core ve SQL etiketlerinde
// böylece farklı kategorilerdeki ve farklı yazarlardaki aynı konulu yazılar bir arada listelenebiliyor
public class Tag : BaseEntity
{
    public string Name { get; set; }
    // çoğu etiket teknoloji adı olduğu için iki dilde aynı, farklı olanlar için (Test / Testing gibi)
    public string? NameEn { get; set; }
    // url'de kullanılan hali: /tag/dotnet  (boşluk, türkçe karakter, nokta yok)
    public string Slug { get; set; }

    // çoka çok (many-to-many) ilişki: bir etiketin birden fazla blogu var
    // ara tabloyu (BlogTags) EF Core kendisi oluşturuyor, ayrı entity yazmadım
    public virtual IList<Blog> Blogs { get; set; }

    // İngilizce ad yoksa Türkçe adı dönüyor
    public string GetName(string language) =>
        language == Languages.English && !string.IsNullOrWhiteSpace(NameEn) ? NameEn : Name;
}
