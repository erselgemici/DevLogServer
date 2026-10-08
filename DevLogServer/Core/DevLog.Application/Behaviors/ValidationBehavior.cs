using FluentValidation;
using MediatR;

namespace DevLog.Application.Behaviors;

// MediatR pipeline'ı, her istek handler'a gitmeden önce buradan geçiyor
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        // bu isteğe ait validator varsa çalıştırıyorum
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResult = await Task.WhenAll(
                validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            var failures = validationResult.Where(result => result is not null)
                .SelectMany(x => x.Errors).ToList();

            if (failures.Any())
            {
                var errorDetails = failures.GroupBy(x => x.PropertyName)
                    .ToDictionary(x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray()).ToList();

                // hata varsa exception fırlatıyorum, middleware yakalayıp 400 dönüyor
                throw new ValidationException(failures);
            }
        }

        // hata yoksa handler'a devam
        return await next(cancellationToken);


    }
}