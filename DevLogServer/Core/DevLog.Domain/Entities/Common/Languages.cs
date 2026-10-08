namespace DevLog.Domain.Entities.Common
{
    // sistemde desteklenen diller, varsayılan dil Türkçe
    public static class Languages
    {
        public const string Default = "tr";
        public const string English = "en";

        public static readonly string[] Supported = [Default, English];

        // "en-US", "EN" gibi gelen değerleri "en" haline getiriyor, desteklenmiyorsa varsayılanı dönüyor
        public static string Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Default;
            }

            var code = value.Trim().ToLowerInvariant();
            if (code.Length > 2)
            {
                code = code[..2];
            }

            return Supported.Contains(code) ? code : Default;
        }
    }
}
