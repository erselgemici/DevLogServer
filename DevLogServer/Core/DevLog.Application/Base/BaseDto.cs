namespace DevLog.Application.Base
{
    // result class'larında ortak alanlar
    public abstract class BaseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
