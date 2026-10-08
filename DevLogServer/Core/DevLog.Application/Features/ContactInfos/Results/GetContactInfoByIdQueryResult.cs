using DevLog.Application.Base;

namespace DevLog.Application.Features.ContactInfos.Result;

// client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
public class GetContactInfoByIdQueryResult : BaseDto
{
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string MapUrl { get; set; }
}
