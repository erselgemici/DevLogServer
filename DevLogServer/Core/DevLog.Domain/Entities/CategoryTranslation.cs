using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities
{
    // kategori adının başka dillerdeki karşılığı
    public class CategoryTranslation : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string LanguageCode { get; set; }
        public string CategoryName { get; set; }
    }
}
