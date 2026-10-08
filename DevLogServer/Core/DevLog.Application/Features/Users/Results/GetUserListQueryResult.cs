namespace DevLog.Application.Features.Users.Result;

// kullanıcı listesinde gösterdiğim bilgiler, şifre gibi alanlar dışarı çıkmıyor
public class GetUserListQueryResult
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? ImageUrl { get; set; }
    public string Role { get; set; }
    public int BlogCount { get; set; }
}
