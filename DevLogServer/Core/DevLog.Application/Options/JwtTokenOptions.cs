namespace DevLog.Application.Options;

// appsettings'teki JwtTokenOptions bölümü bu class'a bağlanıyor
public class JwtTokenOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    // key'i github'a koymamak için user secrets'ta tutuyorum
    public string Key { get; set; }
    public int ExpireInMinutes { get; set; }
}
