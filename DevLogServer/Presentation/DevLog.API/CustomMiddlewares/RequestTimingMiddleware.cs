using System.Diagnostics;

namespace DevLog.API.CustomMiddlewares
{
    // her isteğin ne kadar sürdüğünü ölçüp yavaş olanları konsola yazıyor
    // "sistem yavaş" dendiğinde hangi endpoint'in yavaş olduğunu tahmin etmek yerine buradan görebiliyorum
    // NOT: Stopwatch, DateTime.Now farkından daha hassas ölçüyor (milisaniye altı)
    public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        // bu süreden uzun süren istekler uyarı olarak loglanıyor
        private const int SlowRequestMs = 500;

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await next(context);

            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;

            if (elapsed > SlowRequestMs)
            {
                // Warning seviyesinde yazıyorum ki appsettings'te Information kapalı olsa da görünsün
                logger.LogWarning("YAVAŞ İSTEK: {Method} {Path} {Elapsed} ms sürdü",
                    context.Request.Method, context.Request.Path, elapsed);
            }
        }
    }
}
