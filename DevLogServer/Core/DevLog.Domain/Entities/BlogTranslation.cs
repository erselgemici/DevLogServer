using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities
{
    // blog yazısının başka bir dildeki karşılığı (ör: en)
    // Türkçe metin Blog tablosunda duruyor, diğer diller burada
    public class BlogTranslation : BaseEntity
    {
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public string LanguageCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
