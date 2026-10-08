using MediatR;
using DevLog.Application.Constants;
using DevLog.Application.Features.Socials.Commands;
using DevLog.Application.Features.Socials.Queries;

namespace DevLog.API.Endpoints;

// minimal api endpointleri, controller yerine bu yapıyı kullandım
// AllowAnonymous: herkes erişebilir, AdminOnly: sadece Admin rolü, ikisi de yoksa giriş yapmış herkes
public static class SocialEndpoints
{
    public static void RegisterSocialEndpoints(this IEndpointRouteBuilder app)
    {
        // hepsi aynı route altında toplansın diye group kullandım
        var group = app.MapGroup("/socials").WithTags("Socials");

        group.MapGet(string.Empty, async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetSocialsQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetSocialByIdQuery(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapPost(string.Empty, async (CreateSocialCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapPut(string.Empty, async (UpdateSocialCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapDelete("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new RemoveSocialCommand(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}
