using DevLog.Application.Base;

namespace DevLog.Application.Features.Messages.Result;

// client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
public class GetReadMessagesQueryResult : BaseDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string MessageBody { get; set; }
    public bool IsRead { get; set; }
}
