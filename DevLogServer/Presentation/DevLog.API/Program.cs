using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using DevLog.API.CustomMiddlewares;
using DevLog.API.Endpoints.Registration;
using DevLog.Application.Extensions;
using DevLog.Persistence.Extensions;
using DevLog.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// NOT: appsettings.json'da "Microsoft.EntityFrameworkCore.Database.Command": "Warning" yaptım.
// Information'dayken EF çalıştırdığı her SQL sorgusunu konsola yazıyordu, yüzlerce log
// API'yi yavaşlatıyordu. sorguları görmek istersem geçici olarak tekrar "Information" yapabilirim

// katmanların servislerini extension metodlarla ekliyorum
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

// angular uygulaması 4200 portundan istek atabilsin diye
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(cfg =>
    {
        cfg.WithOrigins("http://localhost:4200")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials();
    });
});

// blog -> category -> blogs gibi döngülerde json hatası vermesin
builder.Services.ConfigureHttpJsonOptions(config =>
{
    config.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddOpenApi();

// cevapları sıkıştırarak (gzip / brotli) gönderiyorum, JSON metin olduğu için boyutu çok küçülüyor
// NOT: https'te varsayılan olarak kapalı (BREACH saldırısı riski yüzünden), token gibi gizli bilgi
// içeren cevaplar dışında sorun olmadığı için açtım
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

var app = builder.Build();

// veritabanı boşsa temadaki örnek içerikleri ekliyor
await DbSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// hata yakalama middleware'i en başta olmalı
app.UseMiddleware<CustomExceptionHandlingMiddleware>();
// yavaş istekleri konsola yazan middleware (süre ölçümü bütün işlemleri kapsasın diye başlarda)
app.UseMiddleware<RequestTimingMiddleware>();
app.UseResponseCompression();
// isteğin dilini (tr/en) belirliyor
app.UseMiddleware<LanguageMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
// önce authentication sonra authorization
app.UseAuthentication();
app.UseAuthorization();

// /api altındaki her şey token istiyor, herkese açık olanları endpointlerde AllowAnonymous yaptım
app.MapGroup("/api")
   .RequireAuthorization()
   .RegisterEndpoints();

app.Run();
