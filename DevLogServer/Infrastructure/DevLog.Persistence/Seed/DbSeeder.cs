using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DevLog.Application.Constants;
using DevLog.Domain.Entities;
using DevLog.Domain.Entities.Common;
using DevLog.Persistence.Context;

namespace DevLog.Persistence.Seed
{
    // proje ilk açıldığında veritabanı boşsa örnek içerikleri (Türkçe + İngilizce) ekliyor
    // DevLog: yazılımcıların yazılım teknolojileri hakkında blog yazdığı platform
    public static class DbSeeder
    {
        private const string DefaultPassword = "Admin123!";
        private const string AdminImage = "assets/img/authors/admin.jpg";
        // admin hesabı platformun editörü olarak görünüyor (kişisel isim yerine)
        private const string AdminFirstName = "DevLog";
        private const string AdminLastName = "Editör";

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            // bekleyen migration varsa önce onları uyguluyorum (veritabanı yoksa oluşturuyor)
            await context.Database.MigrateAsync();

            // admin'in profil resmi ve adı sonradan değiştiği için, veritabanı silinmeden de güncellensin
            var existingAdmin = await userManager.FindByEmailAsync("admin@devlog.com");
            if (existingAdmin is not null &&
                (existingAdmin.ImageUrl != AdminImage || existingAdmin.FirstName != AdminFirstName || existingAdmin.LastName != AdminLastName))
            {
                existingAdmin.ImageUrl = AdminImage;
                existingAdmin.FirstName = AdminFirstName;
                existingAdmin.LastName = AdminLastName;
                await userManager.UpdateAsync(existingAdmin);
            }

            // roller her açılışta kontrol ediliyor, yoksa oluşturuluyor
            await EnsureRolesAsync(roleManager, userManager);

            // daha önce blog eklendiyse tekrar seed yapmıyorum
            if (await context.Blogs.AnyAsync())
            {
                return;
            }

            // yazarlar (fotoğraflar cinsiyete uygun seçildi, DevBot için robot avatar)
            var authors = new Dictionary<string, AppUser>
            {
                ["admin"] = await GetOrCreateUserAsync(userManager, "admin", "admin@devlog.com", AdminFirstName, AdminLastName, AdminImage, Roles.Admin),
                ["can"] = await GetOrCreateUserAsync(userManager, "can", "can@devlog.com", "Can", "Demir", "assets/img/authors/can.jpg"),
                ["mert"] = await GetOrCreateUserAsync(userManager, "mert", "mert@devlog.com", "Mert", "Özkan", "assets/img/authors/mert.jpg"),
                ["burak"] = await GetOrCreateUserAsync(userManager, "burak", "burak@devlog.com", "Burak", "Yıldız", "assets/img/authors/burak.jpg"),
                ["emre"] = await GetOrCreateUserAsync(userManager, "emre", "emre@devlog.com", "Emre", "Şahin", "assets/img/authors/emre.jpg"),
                ["kaan"] = await GetOrCreateUserAsync(userManager, "kaan", "kaan@devlog.com", "Kaan", "Arslan", "assets/img/authors/kaan.jpg"),
                ["elif"] = await GetOrCreateUserAsync(userManager, "elif", "elif@devlog.com", "Elif", "Yılmaz", "assets/img/authors/elif.jpg"),
                ["zeynep"] = await GetOrCreateUserAsync(userManager, "zeynep", "zeynep@devlog.com", "Zeynep", "Kaya", "assets/img/authors/zeynep.jpg"),
                ["selin"] = await GetOrCreateUserAsync(userManager, "selin", "selin@devlog.com", "Selin", "Aksoy", "assets/img/authors/selin.jpg"),
                ["ayse"] = await GetOrCreateUserAsync(userManager, "ayse", "ayse@devlog.com", "Ayşe", "Çelik", "assets/img/authors/ayse.jpg"),
                ["ece"] = await GetOrCreateUserAsync(userManager, "ece", "ece@devlog.com", "Ece", "Doğan", "assets/img/authors/ece.jpg"),
                ["devbot"] = await GetOrCreateUserAsync(userManager, "devbot", "devbot@devlog.com", "DevBot", "", "assets/img/authors/devbot.svg"),
                // yabancı yazarlar
                ["emma"] = await GetOrCreateUserAsync(userManager, "emma", "emma@devlog.com", "Emma", "Johnson", "assets/img/authors/emma.jpg"),
                ["sofia"] = await GetOrCreateUserAsync(userManager, "sofia", "sofia@devlog.com", "Sofia", "Martínez", "assets/img/authors/sofia.jpg"),
                ["lukas"] = await GetOrCreateUserAsync(userManager, "lukas", "lukas@devlog.com", "Lukas", "Müller", "assets/img/authors/lukas.jpg"),
                ["daniel"] = await GetOrCreateUserAsync(userManager, "daniel", "daniel@devlog.com", "Daniel", "Okafor", "assets/img/authors/daniel.jpg"),
                ["arjun"] = await GetOrCreateUserAsync(userManager, "arjun", "arjun@devlog.com", "Arjun", "Mehta", "assets/img/authors/arjun.jpg"),
            };

            // kategoriler (Türkçe ad + İngilizce çeviri)
            var categories = new Dictionary<string, Category>
            {
                ["history"] = CreateCategory("Bilişim Tarihi", "Computing History"),
                ["languages"] = CreateCategory("Programlama Dilleri", "Programming Languages"),
                ["web"] = CreateCategory("Web Teknolojileri", "Web Technologies"),
                ["databases"] = CreateCategory("Veritabanları", "Databases"),
                ["architecture"] = CreateCategory("Yazılım Mimarisi", "Software Architecture"),
                ["devops"] = CreateCategory("DevOps ve Bulut", "DevOps & Cloud"),
                ["frontend"] = CreateCategory("Frontend Geliştirme", "Frontend Development"),
                ["backend"] = CreateCategory("Backend Geliştirme", "Backend Development"),
                ["mobile"] = CreateCategory("Mobil Geliştirme", "Mobile Development"),
                ["security"] = CreateCategory("Güvenlik", "Security"),
                ["testing"] = CreateCategory("Test ve Kalite", "Testing & Quality"),
                ["ai-data"] = CreateCategory("Yapay Zekâ ve Veri", "AI & Data"),
            };
            await context.Categories.AddRangeAsync(categories.Values);
            await context.SaveChangesAsync();

            // etiketler (konular): slug -> Tag sözlüğü, bloglara bağlarken slug ile buluyorum
            var tags = SeedData.Tags.ToDictionary(
                x => x.Slug,
                x => new Tag { Slug = x.Slug, Name = x.Name, NameEn = x.NameEn });
            await context.Tags.AddRangeAsync(tags.Values);
            await context.SaveChangesAsync();

            // bloglar
            var blogs = new List<Blog>();
            foreach (var item in SeedData.Blogs)
            {
                var blog = new Blog
                {
                    Title = item.TitleTr,
                    Description = item.DescriptionTr,
                    CoverImage = item.CoverImage,
                    BlogImage = item.BlogImage,
                    CategoryId = categories[item.CategoryKey].Id,
                    UserId = authors[item.AuthorKey].Id,
                    // yazının etiketleri, ara tabloya (BlogTags) EF kendisi yazıyor
                    Tags = item.Tags.Select(slug => tags[slug]).ToList()
                };
                blog.SetTranslation(Languages.English, item.TitleEn, item.DescriptionEn);
                blogs.Add(blog);
            }

            await context.Blogs.AddRangeAsync(blogs);
            await context.SaveChangesAsync();

            // interceptor CreatedAt'i şu anki zaman yapıyor, tarihler farklı görünsün diye
            // ExecuteUpdate ile direkt güncelliyorum (ExecuteUpdate interceptor'a takılmıyor)
            for (int i = 0; i < blogs.Count; i++)
            {
                var date = DateTime.Now.AddDays(-SeedData.Blogs[i].DaysAgo);
                var id = blogs[i].Id;
                await context.Blogs.Where(b => b.Id == id)
                    .ExecuteUpdateAsync(s => s.SetProperty(b => b.CreatedAt, date).SetProperty(b => b.UpdatedAt, date));
            }

            // yorumlar: her yazıya kendi konusuyla ilgili yorumlar ve yazarın cevapları
            // seed verisinde yorumlar blog key'i ile tutuluyor, key -> (blog, seed) sözlüğü kuruyorum
            var blogByKey = new Dictionary<string, (Blog Blog, SeedData.BlogSeed Seed)>();
            for (int i = 0; i < blogs.Count; i++)
            {
                blogByKey[SeedData.Blogs[i].Key] = (blogs[i], SeedData.Blogs[i]);
            }

            var comments = new List<Comment>();
            var replies = new List<SubComment>();

            foreach (var group in SeedData.Comments.GroupBy(x => x.BlogKey))
            {
                var (blog, seed) = blogByKey[group.Key];
                var author = authors[seed.AuthorKey];
                var blogDate = DateTime.Now.AddDays(-seed.DaysAgo);

                // yorumları yazının yayınlandığı tarihten sonraki ilk 3 güne (yazı daha yeniyse bugüne kadar) yayıyorum
                var span = TimeSpan.FromHours(Math.Min((DateTime.Now - blogDate).TotalHours - 1, 72));
                var items = group.ToList();

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    var commentDate = blogDate + span * (i + 1) / (items.Count + 1);

                    var comment = new Comment
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = ToEmail(item.FirstName, item.LastName),
                        Body = item.Body,
                        CommentDate = commentDate,
                        BlogId = blog.Id
                    };
                    comments.Add(comment);

                    // yazar bu yoruma cevap verdiyse alt yorum olarak ekliyorum
                    if (!string.IsNullOrWhiteSpace(item.AuthorReply))
                    {
                        var replyDate = commentDate.AddHours(3);
                        replies.Add(new SubComment
                        {
                            FirstName = author.FirstName,
                            LastName = author.LastName,
                            Email = author.Email!,
                            Body = item.AuthorReply,
                            // cevap tarihi bugünü geçmesin
                            CommentDate = replyDate > DateTime.Now ? DateTime.Now.AddMinutes(-10) : replyDate,
                            Comment = comment
                        });
                    }
                }
            }

            await context.Comments.AddRangeAsync(comments);
            await context.SubComments.AddRangeAsync(replies);
            await context.SaveChangesAsync();

            // iletişim bilgileri
            if (!await context.ContactInfos.AnyAsync())
            {
                await context.ContactInfos.AddAsync(new ContactInfo
                {
                    Address = "Büyükdere Cad. No:185, Levent, Beşiktaş / İstanbul",
                    Email = "info@devlog.com",
                    Phone = "+90 (212) 555 01 01",
                    MapUrl = "https://maps.google.com/maps?q=Levent%2C%20Istanbul&z=14&output=embed"
                });
            }

            // sosyal medya linkleri (footer ve header'da gösteriliyor)
            if (!await context.Socials.AnyAsync())
            {
                await context.Socials.AddRangeAsync(
                    new Social { Title = "X", Url = "https://x.com", Icon = "bi bi-twitter-x" },
                    new Social { Title = "Instagram", Url = "https://instagram.com", Icon = "bi bi-instagram" },
                    new Social { Title = "LinkedIn", Url = "https://linkedin.com", Icon = "bi bi-linkedin" },
                    new Social { Title = "GitHub", Url = "https://github.com", Icon = "bi bi-github" });
            }

            // admin panelde görünsün diye örnek mesajlar
            if (!await context.Messages.AnyAsync())
            {
                await context.Messages.AddRangeAsync(SeedData.Messages.Select(m => new Message
                {
                    Name = m.Name,
                    Email = m.Email,
                    Subject = m.Subject,
                    MessageBody = m.Body,
                    IsRead = m.IsRead
                }));
            }

            await context.SaveChangesAsync();
        }

        // Admin ve Writer rollerini oluşturuyor, admin'i Admin rolüne, rolü olmayanları Writer rolüne ekliyor
        private static async Task EnsureRolesAsync(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            foreach (var roleName in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole { Id = Guid.NewGuid().ToString(), Name = roleName });
                }
            }

            foreach (var user in userManager.Users.ToList())
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Count > 0) continue;

                var role = user.Email == "admin@devlog.com" ? Roles.Admin : Roles.Writer;
                await userManager.AddToRoleAsync(user, role);
            }
        }

        private static Category CreateCategory(string nameTr, string nameEn)
        {
            var category = new Category { CategoryName = nameTr };
            category.SetTranslation(Languages.English, nameEn);
            return category;
        }

        // yorum yapanlar için örnek e-posta: "Gökhan Şimşek" -> gokhan.simsek@example.com
        // türkçe karakterleri ingilizce karşılıklarına çeviriyorum, e-postada sorun çıkmasın
        private static string ToEmail(string firstName, string lastName)
        {
            var text = $"{firstName}.{lastName}".ToLowerInvariant().Replace(" ", "");
            var map = new Dictionary<char, char>
            {
                ['ç'] = 'c', ['ğ'] = 'g', ['ı'] = 'i', ['ö'] = 'o', ['ş'] = 's', ['ü'] = 'u',
                ['á'] = 'a', ['é'] = 'e', ['í'] = 'i', ['ó'] = 'o', ['ú'] = 'u', ['ñ'] = 'n', ['ä'] = 'a'
            };
            // "İ" küçültülünce i + nokta işareti (\u0307) oluyor, o işareti atıyorum
            var chars = text.Where(c => c != '\u0307')
                .Select(c => map.TryGetValue(c, out var r) ? r : c).ToArray();
            return new string(chars) + "@example.com";
        }

        private static async Task<AppUser> GetOrCreateUserAsync(UserManager<AppUser> userManager, string userName,
            string email, string firstName, string lastName, string imageUrl, string role = Roles.Writer)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                return user;
            }

            user = new AppUser
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                ImageUrl = imageUrl
            };

            var result = await userManager.CreateAsync(user, DefaultPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(user, role);

            return user;
        }
    }
}
