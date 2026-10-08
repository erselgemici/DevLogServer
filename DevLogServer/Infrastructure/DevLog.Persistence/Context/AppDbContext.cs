using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DevLog.Domain.Entities;

namespace DevLog.Persistence.Context
{
    // IdentityDbContext'ten türettim, identity tabloları da bu context'te oluşuyor
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, string>(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogTranslation> BlogTranslations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // identity tablolarının ayarları için base mutlaka çağrılmalı
            base.OnModelCreating(builder);

            // bir blogun aynı dilde iki çevirisi olmasın
            builder.Entity<BlogTranslation>(e =>
            {
                e.Property(x => x.LanguageCode).HasMaxLength(5);
                e.HasIndex(x => new { x.BlogId, x.LanguageCode }).IsUnique();
            });

            builder.Entity<CategoryTranslation>(e =>
            {
                e.Property(x => x.LanguageCode).HasMaxLength(5);
                e.HasIndex(x => new { x.CategoryId, x.LanguageCode }).IsUnique();
            });

            // etiketler: url'de kullanılan slug tekrar etmesin
            builder.Entity<Tag>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(50);
                e.Property(x => x.NameEn).HasMaxLength(50);
                e.Property(x => x.Slug).HasMaxLength(50);
                e.HasIndex(x => x.Slug).IsUnique();
            });

            // blog <-> etiket çoka çok ilişki
            // NOT: ara tablo için entity yazmadım, EF Core 5'ten beri UsingEntity ile kendisi oluşturuyor.
            // tablo adını vermezsem "BlogTag" diyor, ben çoğul olsun diye BlogTags yaptım
            builder.Entity<Blog>()
                .HasMany(x => x.Tags)
                .WithMany(x => x.Blogs)
                .UsingEntity(j => j.ToTable("BlogTags"));
        }
    }
}
