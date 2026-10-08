using System.Text.RegularExpressions;

namespace DevLog.Application.Helpers;

// liste sayfalarında yazının tamamı yerine kısa bir özet göndermek için yazdım
// içerikte ## başlık, - liste, ``` kod bloğu gibi işaretler var, özette bunlar görünmesin diye temizliyorum
// static class: nesne oluşturmadan TextHelper.Summarize(...) diye direkt çağrılıyor
// NOT: Regex kalıplarında \s = boşluk, \S = boşluk olmayan, *? = olabildiğince az eşleş (lazy)
public static class TextHelper
{
    public static string Summarize(string? text, int length = 220)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var plain = Regex.Replace(text, "```[\\s\\S]*?```", " ");
        plain = Regex.Replace(plain, "^## .*$", " ", RegexOptions.Multiline);
        plain = Regex.Replace(plain, "^- ", "", RegexOptions.Multiline);
        plain = plain.Replace("`", "").Replace("**", "");
        plain = Regex.Replace(plain, "\\s+", " ").Trim();

        if (plain.Length <= length) return plain;

        // kelimenin ortasından kesmemek için son boşluğa kadar alıyorum
        // plain[..length] = Substring(0, length) ile aynı şey (C# 8 range operatörü)

        var cut = plain[..length];
        var lastSpace = cut.LastIndexOf(' ');
        return (lastSpace > 0 ? cut[..lastSpace] : cut) + "...";
    }

    // ortalama okuma hızı dakikada ~200 kelime kabul edilip okuma süresi hesaplanıyor
    // kod blokları da sayılıyor, kod okumak daha yavaş olduğu için bu kabul edilebilir
    public static int ReadingMinutes(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 1;

        var wordCount = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        // en az 1 dakika göstersin, 0 dk okuma garip duruyor
        return Math.Max(1, (int)Math.Round(wordCount / 200.0));
    }
}
