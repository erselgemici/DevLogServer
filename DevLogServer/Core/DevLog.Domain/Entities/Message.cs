using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities;

// iletişim sayfasından gelen mesajlar
public class Message: BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string MessageBody { get; set; }
    // admin panelde okundu/okunmadı ayırmak için
    public bool IsRead { get; set; }
}