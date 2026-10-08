using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DevLog.Application.Constants;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Options;
using DevLog.Domain.Entities;
using DevLog.Persistence.Concrete;
using DevLog.Persistence.Context;
using DevLog.Persistence.Interceptors;

namespace DevLog.Persistence.Extensions
{
    public static class ServiceRegistrations
    {
        // veritabanı, identity ve jwt ayarları
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                // connection string user secrets'ta
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
                // CreatedAt/UpdatedAt için
                options.AddInterceptors(new AuditDbContextInterceptor());
                // NOT: kursta lazy loading açıktı (UseLazyLoadingProxies). ilişkili tablolar ihtiyaç olunca
                // kendiliğinden yükleniyordu ama her erişim ayrı bir sorgu demek (N+1) ve nerede sorgu
                // atıldığı koddan görünmüyordu. veri çoğalınca sistem çok yavaşladı.
                // kapattım: artık her handler ihtiyacı olan tabloyu Include ile kendisi istiyor.
                // Include edilmeyen ilişki null geliyor (sessizce sorgu atmıyor), hatayı bulmak da kolay
                // options.UseLazyLoadingProxies();
            });

            // aynı mail ile ikinci kayıt olmasın
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // generic repository'i bütün entity'ler için kaydediyor
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IJwtService, JwtService>();

            var jwtTokenOptions = configuration.GetSection(nameof(JwtTokenOptions)).Get<JwtTokenOptions>()
                ?? throw new InvalidOperationException("JwtTokenOptions ayarları appsettings / user secrets içinde bulunamadı.");

            // identity default olarak cookie kullanıyor, api olduğu için jwt'yi default yaptım
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                // claim isimleri token'daki gibi kalsın (sub, name, role), .NET uzun isimlere çevirmesin
                opt.MapInboundClaims = false;
                // gelen token'ı kontrol ederken nelere bakılacağı
                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtTokenOptions.Issuer,
                    ValidAudience = jwtTokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions.Key)),
                    // süre bitince hemen geçersiz olsun
                    ClockSkew = TimeSpan.Zero,
                    // [Authorize(Roles = ...)] ve RequireRole token'daki "role" claim'ine baksın
                    RoleClaimType = "role",
                    NameClaimType = "name",
                };
            });

            // sadece Admin rolündekilerin erişebileceği endpoint'ler için politika
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.AdminOnly, policy => policy.RequireRole(Roles.Admin));
            });
        }
    }
}
