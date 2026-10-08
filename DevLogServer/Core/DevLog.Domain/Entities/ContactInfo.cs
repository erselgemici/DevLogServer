using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities;

// iletişim sayfasında gösterilen adres, mail, telefon bilgileri
public class ContactInfo: BaseEntity
{
    public string Address  { get; set; }
    public string Email  { get; set; }
    public string Phone  { get; set; }
    public string MapUrl  { get; set; }
}