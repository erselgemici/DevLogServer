namespace DevLog.Application.Constants
{
    // sistemdeki roller
    // Admin: her şeyi yönetebilir
    // Writer (yazar): sadece blog yazabilir, kendi yazılarını düzenleyip silebilir
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Writer = "Writer";

        public static readonly string[] All = [Admin, Writer];
    }

    // endpoint'lerde kullandığım yetki politikaları
    public static class Policies
    {
        public const string AdminOnly = "AdminOnly";
    }
}
