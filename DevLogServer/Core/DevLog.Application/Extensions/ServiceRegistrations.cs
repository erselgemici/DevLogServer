using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DevLog.Application.Behaviors;
using DevLog.Application.Options;

namespace DevLog.Application.Extensions
{
    public static class ServiceRegistrations
    {
        // Application katmanının servislerini Program.cs'i kalabalık etmemek için burada ekliyorum
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // bu projedeki bütün Profile class'larını buluyor
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

            // handler'ları otomatik kaydediyor, validation behavior'ı da pipeline'a ekledim
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            // bütün validator'lar
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // jwt ayarlarını IOptions ile kullanabilmek için
            services.Configure<JwtTokenOptions>(configuration.GetSection(nameof(JwtTokenOptions)));
        }
    }
}
