using FluentValidation;
using DevLog.Application.Base;

namespace DevLog.API.CustomMiddlewares
{
    // bütün hataları tek yerden yakalamak için yazdığım middleware
    public class CustomExceptionHandlingMiddleware(RequestDelegate next)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }

            // validation hatalarını 400 olarak ve alan adına göre gruplayarak dönüyorum
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new BaseResult<object>()
                {
                    Errors = ex.Errors.GroupBy(x => x.PropertyName)
                        .Select(group => new Dictionary<string, string>
                        {
                            {group.Key,
                            group.Select(x=>x.ErrorMessage).FirstOrDefault()
                            }
                        }
                        ).ToList()
                };

                await context.Response.WriteAsJsonAsync(response);

                
            }

            // beklenmeyen diğer hatalar 500
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { errorMessage = ex.Message });
            }
        }


    }
}
