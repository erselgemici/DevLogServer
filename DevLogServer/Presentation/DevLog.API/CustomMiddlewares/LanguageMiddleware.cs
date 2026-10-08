using DevLog.Application.Localization;

namespace DevLog.API.CustomMiddlewares
{
    // her istekte Accept-Language header'ına bakıp isteğin dilini belirliyor (tr / en)
    // header yoksa ya da desteklenmeyen bir dilse Türkçe kullanılıyor
    public class LanguageMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            LanguageContext.Current = context.Request.Headers.AcceptLanguage.ToString();
            await next(context);
        }
    }
}
