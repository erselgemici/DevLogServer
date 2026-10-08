using MediatR;
using DevLog.Application.Features.Tags.Queries;

namespace DevLog.API.Endpoints;

// etiket (konu) endpointleri, herkes görebiliyor
public static class TagEndpoints
{
    public static void RegisterTagEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tags").WithTags("Tags");

        group.MapGet(string.Empty, async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetTagsQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();
    }
}
