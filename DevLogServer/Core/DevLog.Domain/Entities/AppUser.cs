using Microsoft.AspNetCore.Identity;

namespace DevLog.Domain.Entities;

// Identity'nin user tablosunu kendi alanlarımla genişlettim
public class AppUser : IdentityUser<string>
{
    public AppUser()
    {
        // Id string olduğu için guid'i string olarak veriyorum
        Id = Guid.NewGuid().ToString();
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ImageUrl { get; set; }
    // kullanıcının yazdığı bloglar
    public virtual IList<Blog> Blogs { get; set; }
}
