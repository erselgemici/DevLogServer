using MediatR;
using DevLog.Application.Constants;
using DevLog.Application.Features.Messages.Commands;
using DevLog.Application.Features.Messages.Queries;

namespace DevLog.API.Endpoints;

// minimal api endpointleri, controller yerine bu yapıyı kullandım
// AllowAnonymous: herkes erişebilir, AdminOnly: sadece Admin rolü, ikisi de yoksa giriş yapmış herkes
public static class MessageEndpoints
{
    public static void RegisterMessageEndpoints(this IEndpointRouteBuilder app)
    {
        // hepsi aynı route altında toplansın diye group kullandım
        var group = app.MapGroup("/messages").WithTags("Messages");

        group.MapGet(string.Empty, async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetMessagesQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetMessageByIdQuery(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapPost(string.Empty, async (CreateMessageCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapPut(string.Empty, async (UpdateMessageCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapDelete("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new RemoveMessageCommand(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapGet("unread", async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetUnreadMessagesQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapGet("read", async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetReadMessagesQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}
