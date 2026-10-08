using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities
{
    // footer'daki sosyal medya linkleri
    public class Social: BaseEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        // bootstrap icon class adı tutuluyor (ör: bi bi-twitter)
        public string Icon { get; set; }

    }
}
