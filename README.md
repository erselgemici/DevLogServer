# DevLog - Yazılımcılar İçin Blog Platformu

Yazılımcıların bilişim tarihinden bugünün teknolojilerine kadar yazı paylaştığı, çok yazarlı ve çok dilli (TR / EN) bir blog platformu.

Backend tarafında **.NET 9 Minimal API**, **Onion Architecture** ve **CQRS + MediatR**; frontend tarafında **Angular 20** kullandım.

> *A multi-author, bilingual (Turkish / English) blog platform for developers. Built with .NET 9 Minimal API, Onion Architecture, CQRS + MediatR, EF Core, ASP.NET Core Identity + JWT on the backend and Angular 20 on the frontend.*

---

## İçindekiler

- [Proje hakkında](#proje-hakkında)
- [Özellikler](#özellikler)
- [Kullanılan teknolojiler](#kullanılan-teknolojiler)
- [Mimari](#mimari)
- [Klasör yapısı](#klasör-yapısı)
- [Kurulum](#kurulum)
- [API uç noktaları](#api-uç-noktaları)
- [Ekran görüntüleri](#ekran-görüntüleri)
- [Bu projede öğrendiklerim](#bu-projede-öğrendiklerim)

---

## Proje hakkında

Proje, bir Udemy eğitimindeki ZenBlog uygulamasını temel alarak başladı. Eğitimdeki mimariyi (Onion Architecture, CQRS, MediatR, Minimal API, Angular) koruyup üzerine kendi eklemelerimi yaptım ve konsepti genişlettim: tek kişilik bir blog yerine **birçok yazarın** yazılım teknolojileri hakkında yazdığı bir platform.

Platformda şu an 12 kategoride, 17 yazardan 87 yazı ve yazılara yapılmış 330'dan fazla yorum bulunuyor. Yazılar bilişim tarihinden (ENIAC, Ada Lovelace, Unix) bugünün konularına (Angular Signals, RAG, Kubernetes, OWASP Top 10) kadar uzanıyor ve hepsi Türkçe ile İngilizce olarak mevcut.

## Özellikler

### Ziyaretçi tarafı
- **Çok dilli arayüz ve içerik (TR / EN):** Arayüz metinleri Angular'da, yazı ve kategori içerikleri veritabanındaki çeviri tablolarında tutuluyor. Seçilen dil `Accept-Language` başlığı ile API'ye gidiyor.
- **Kategori ve etiket (konu) sayfaları:** Bir yazının tek kategorisi, birden fazla etiketi var. Örneğin `#.NET` sayfasında farklı kategorilerdeki ve farklı yazarlardaki bütün .NET yazıları bir arada listeleniyor.
- **Sayfalama:** Tekrar kullanılabilir pagination component'i; sayfa numarası URL'de (`?page=2`) tutuluyor.
- **Anlık arama:** RxJS ile (`debounceTime`, `distinctUntilChanged`, `switchMap`) yazdıkça sonuç getiren arama sayfası.
- **Yazı detay sayfası:**
  - Okuma süresi ve sayfanın üstünde okuma ilerleme çubuğu
  - Ara başlıklardan otomatik oluşan, kaydırırken aktif başlığı gösteren içindekiler menüsü
  - highlight.js ile renklendirilmiş kod blokları ve "Kopyala" butonu
  - "Bu konudaki diğer yazılar": ortak etiketlere göre ilgili yazılar
  - Yorumlar ve yazarın yorumlara verdiği cevaplar
- **Basit markdown desteği:** Yazılarda `## başlık`, `- liste`, kod blokları ve satır içi kod kullanılabiliyor. İçerik HTML'e çevrilmeden önce escape ediliyor (XSS'e karşı).
- İletişim formu, sosyal medya ve iletişim bilgileri veritabanından geliyor.

### Yönetim paneli
- **Rol bazlı yetkilendirme:** `Admin` her şeyi yönetebiliyor; `Writer` (yazar) sadece kendi yazılarını görüp düzenleyebiliyor. Bu kontrol sadece arayüzde değil API tarafında da yapılıyor.
- **Genel bakış (dashboard):** Yazı, kategori, konu, yorum, okunmamış mesaj ve kullanıcı sayıları; kategorilere göre yazı grafiği; en çok yazan yazarlar; son yorumlar.
- Blog, kategori, yorum, mesaj, iletişim bilgisi ve sosyal medya yönetimi (CRUD)
- Blog formunda etiket seçimi ve İngilizce çeviri alanları
- Kullanıcı listesi ve rol değiştirme

## Kullanılan teknolojiler

### Backend
| Teknoloji | Kullanım amacı |
|---|---|
| .NET 9 / ASP.NET Core Minimal API | Controller yerine endpoint grupları (`MapGroup`) |
| Onion Architecture | Domain, Application, Persistence ve API katmanları |
| CQRS + MediatR 12 | Her işlem için ayrı Command / Query ve Handler |
| MediatR Pipeline Behavior | Handler'dan önce otomatik validasyon (`ValidationBehavior`) |
| FluentValidation | Command ve query doğrulama kuralları |
| AutoMapper | Entity ile DTO arasındaki dönüşümler |
| Entity Framework Core 9 + SQL Server | Code-First, migration'lar, `Include` / `AsSplitQuery` ile eager loading |
| Generic Repository + Unit of Work | Veri erişimini tek yerden yönetmek |
| SaveChanges Interceptor | `CreatedAt` / `UpdatedAt` alanlarını otomatik doldurmak |
| ASP.NET Core Identity | Kullanıcı ve rol yönetimi (`AppUser`, `AppRole`) |
| JWT Bearer Authentication | Token tabanlı giriş, rol claim'leri, `AdminOnly` policy |
| Custom Middleware | Global hata yakalama ve istek dilini belirleme |
| User Secrets | Bağlantı cümlesi ve JWT anahtarı repoda tutulmuyor |
| OpenAPI + Scalar | API dokümantasyonu ve test arayüzü |

### Frontend
| Teknoloji | Kullanım amacı |
|---|---|
| Angular 20 (NgModule) | SPA, routing, component yapısı |
| RxJS | HTTP istekleri, anlık arama akışı |
| HttpInterceptor | Her isteğe JWT token ve `Accept-Language` eklemek |
| Route Guards | `AuthGuard`, `AdminGuard` ile sayfa koruması |
| Custom Pipes | Çeviri (`t`), içerik render (`content`), özet (`excerpt`), güvenli URL |
| @auth0/angular-jwt | Token çözme ve süre kontrolü |
| Bootstrap 5, Bootstrap Icons | Arayüz |
| highlight.js | Kod renklendirme |
| Swiper, AOS | Slider ve kaydırma animasyonları |
| Alertify, SweetAlert2 | Bildirimler ve onay pencereleri |

## Mimari

```
            ┌──────────────────────────┐
            │   Angular 20 (Client)    │
            └────────────┬─────────────┘
                         │ HTTP + JWT
            ┌────────────▼─────────────┐
            │  DevLog.API             │  Minimal API endpoint'leri, middleware'ler
            └────────────┬─────────────┘
                         │ MediatR (Send)
            ┌────────────▼─────────────┐
            │  DevLog.Application     │  Command / Query / Handler, DTO, Validator, Mapping
            └────────────┬─────────────┘
                         │ IRepository, IUnitOfWork
            ┌────────────▼─────────────┐
            │  DevLog.Persistence     │  EF Core, DbContext, Repository, JWT, Seed
            └────────────┬─────────────┘
            ┌────────────▼─────────────┐
            │  DevLog.Domain          │  Entity'ler (hiçbir katmana bağımlı değil)
            └──────────────────────────┘
```

Bir isteğin yolculuğu (örnek: yazı listesi):

1. Angular, `BlogService.getAll()` ile `GET /api/blogs` isteği atar. Interceptor token'ı ve dil bilgisini ekler.
2. Endpoint isteği sadece MediatR'a iletir: `mediator.Send(new GetBlogsQuery())`.
3. Varsa validator çalışır, ardından `GetBlogsQueryHandler` repository üzerinden veriyi ilişkileriyle birlikte tek seferde çeker.
4. AutoMapper entity'leri DTO'ya çevirir; başlık ve özet isteğin diline göre doldurulur.
5. Sonuç `BaseResult<T>` ile standart bir formatta döner.

## Klasör yapısı

```
├── DevLogServer
│   ├── Core
│   │   ├── DevLog.Domain          # Entity'ler: Blog, Category, Tag, Comment, AppUser...
│   │   └── DevLog.Application     # Features/<Özellik>/Commands, Queries, Handlers, Results, Validators, Mappings
│   ├── Infrastructure
│   │   └── DevLog.Persistence     # AppDbContext, Migrations, Repository, UnitOfWork, JwtService, Seed
│   └── Presentation
│       └── DevLog.API             # Program.cs, Endpoints, CustomMiddlewares
└── DevLogClient
    └── src/app
        ├── _main-components        # ana sayfa, yazı detayı, kategori, etiket, arama...
        ├── _admin-components       # dashboard ve yönetim sayfaları
        ├── _layouts                # site ve admin layout'ları
        ├── _services, _models      # API servisleri ve modeller
        ├── _guards, _interceptors  # yetki kontrolü, token ekleme
        ├── _pipes, _i18n           # pipe'lar ve TR/EN çeviriler
```

## Kurulum

### Gereksinimler
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server veya SQL Server LocalDB (Visual Studio ile geliyor)
- [Node.js 20+](https://nodejs.org) ve Angular CLI 20 (`npm install -g @angular/cli`)

### 1. Backend

Bağlantı cümlesi ve JWT anahtarı güvenlik için repoda değil, **User Secrets** içinde tutuluyor. API projesinin klasöründe:

```bash
cd DevLogServer/Presentation/DevLog.API

dotnet user-secrets set "ConnectionStrings:SqlConnection" "Server=(localdb)\\MSSQLLocalDB;Database=DevLogDb;Trusted_Connection=True;TrustServerCertificate=True"
dotnet user-secrets set "JwtTokenOptions:Key" "en-az-32-karakterlik-uzun-ve-rastgele-bir-anahtar"

dotnet run --launch-profile https
```

Uygulama ilk açılışta migration'ları uygular, veritabanını oluşturur ve örnek verileri (yazarlar, kategoriler, etiketler, yazılar, yorumlar) ekler.

- API: `https://localhost:7000`
- API dokümantasyonu (Scalar): `https://localhost:7000/scalar`

### 2. Frontend

```bash
cd DevLogClient
npm install
npm start
```

Site `http://localhost:4200` adresinde açılır.


## API uç noktaları

Bazı örnekler (tam liste Scalar arayüzünde):

| Metot | Adres | Açıklama | Yetki |
|---|---|---|---|
| GET | `/api/blogs` | Yazı listesi (özet ve okuma süresiyle) | Herkes |
| GET | `/api/blogs/{id}` | Yazı detayı, yorumlar ve etiketler | Herkes |
| GET | `/api/blogs/{id}/related` | Ortak etiketlere göre ilgili yazılar | Herkes |
| GET | `/api/blogs/byTag/{slug}` | Bir konudaki bütün yazılar | Herkes |
| GET | `/api/blogs/search?q=` | Başlık, içerik ve etiketlerde arama | Herkes |
| POST | `/api/blogs` | Yazı ekleme (yazar token'dan alınır) | Giriş yapmış |
| PUT | `/api/blogs` | Yazı güncelleme (sadece sahibi veya admin) | Giriş yapmış |
| GET | `/api/tags` | Etiketler ve yazı sayıları | Herkes |
| GET | `/api/dashboard` | Yönetim paneli istatistikleri | Giriş yapmış |
| POST | `/api/users/login` | Giriş, JWT döner | Herkes |
| PUT | `/api/users/role` | Kullanıcı rolünü değiştirme | Admin |

## Ekran görüntüleri



## Bu projede öğrendiklerim

- **N+1 problemi:** Lazy loading açıkken listelerde her kayıt için ayrı sorgu atıldığını fark ettim. Lazy loading'i kapatıp her sorguda ihtiyaç duyulan ilişkileri `Include` / `AsSplitQuery` ile açıkça istedim; sorgu sayısı yüzlerden birkaça indi. Sadece sayı gereken yerlerde `Count()` ile SQL'de `COUNT` kullandım.
- **Performans ölçümü:** Yavaş istekleri tahmin etmek yerine ölçmek için süreyi loglayan bir middleware yazdım; ayrıca JSON cevaplarını sıkıştırdım (response compression) ve Angular tarafında sık kullanılan listeleri `shareReplay` ile önbelleğe aldım.
- **DTO tasarımı:** Blog, kategori ve yorum DTO'larının birbirini içermesi döngüsel referans ve megabaytlarca JSON üretiyordu. Her ekran için sadece gereken alanları taşıyan küçük DTO'lar yazdım.
- **Yetkilendirmenin backend'de yapılması:** Yazarın sadece kendi yazısını düzenleyebilmesini arayüzde gizlemek yetmiyor; asıl kontrol handler'da, token'daki kullanıcıya göre yapılıyor.
- **Çok dillilik:** Sabit metinleri frontend'de, kullanıcı içeriğini veritabanında çeviri tablolarıyla tutmak.
- **RxJS ile istek yönetimi:** `switchMap` ile eski arama isteklerini iptal etmek ve `unsubscribe` ile memory leak'i önlemek.
- **Gizli bilgilerin yönetimi:** Bağlantı cümlesi ve JWT anahtarını User Secrets'a taşımak.



[LinkedIn](https://www.linkedin.com/in/erselgemici) · [GitHub](https://github.com/erselgemici)
