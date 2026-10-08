namespace DevLog.Domain.Entities.Common
{
    // bütün entity'lerde ortak olan alanlar, her tabloya tek tek yazmamak için
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
