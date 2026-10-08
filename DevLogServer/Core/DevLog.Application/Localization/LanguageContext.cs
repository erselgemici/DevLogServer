using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Localization
{
    // o anki isteğin dili. API'deki middleware Accept-Language header'ından okuyup buraya yazıyor,
    // AsyncLocal sayesinde her istek kendi değerini görüyor (istekler birbirine karışmıyor)
    public static class LanguageContext
    {
        private static readonly AsyncLocal<string?> _current = new();

        public static string Current
        {
            get => _current.Value ?? Languages.Default;
            set => _current.Value = Languages.Normalize(value);
        }
    }
}
