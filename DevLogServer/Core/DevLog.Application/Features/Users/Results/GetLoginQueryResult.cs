namespace DevLog.Application.Features.Users.Result;

// client'a döndüğüm dto, entity'i direkt dışarı açmamak için ayrı class yaptım
public class GetLoginQueryResult
{
    public string Token { get; set; }
    public DateTime ExpirationTime { get; set; }
}
