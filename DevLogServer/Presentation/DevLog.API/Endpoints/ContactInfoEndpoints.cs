using MediatR;
using DevLog.Application.Constants;
using DevLog.Application.Features.ContactInfos.Commands;
using DevLog.Application.Features.ContactInfos.Queries;

namespace DevLog.API.Endpoints;

// minimal api endpointleri, controller yerine bu yapıyı kullandım
// AllowAnonymous: herkes erişebilir, AdminOnly: sadece Admin rolü, ikisi de yoksa giriş yapmış herkes
public static class ContactInfoEndpoints
{
    public static void RegisterContactInfoEndpoints(this IEndpointRouteBuilder app)
    {
        // hepsi aynı route altında toplansın diye group kullandım
        var group = app.MapGroup("/contactInfos").WithTags("ContactInfos");

        group.MapGet(string.Empty, async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetContactInfosQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetContactInfoByIdQuery(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapPost(string.Empty, async (CreateContactInfoCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapPut(string.Empty, async (UpdateContactInfoCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapDelete("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new RemoveContactInfoCommand(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}
