using MediatR;
using DevLog.Application.Constants;
using DevLog.Application.Features.SubComments.Commands;
using DevLog.Application.Features.SubComments.Queries;

namespace DevLog.API.Endpoints;

// minimal api endpointleri, controller yerine bu yapıyı kullandım
// AllowAnonymous: herkes erişebilir, AdminOnly: sadece Admin rolü, ikisi de yoksa giriş yapmış herkes
public static class SubCommentEndpoints
{
    public static void RegisterSubCommentEndpoints(this IEndpointRouteBuilder app)
    {
        // hepsi aynı route altında toplansın diye group kullandım
        var group = app.MapGroup("/subComments").WithTags("SubComments");

        group.MapGet(string.Empty, async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetSubCommentsQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetSubCommentByIdQuery(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapPost(string.Empty, async (CreateSubCommentCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapPut(string.Empty, async (UpdateSubCommentCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapDelete("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new RemoveSubCommentCommand(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}
