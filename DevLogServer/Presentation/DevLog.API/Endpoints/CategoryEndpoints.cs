using MediatR;
using DevLog.Application.Constants;
using DevLog.Application.Features.Categories.Commands;
using DevLog.Application.Features.Categories.Queries;

namespace DevLog.API.Endpoints;

// minimal api endpointleri, controller yerine bu yapıyı kullandım
// AllowAnonymous: herkes erişebilir, AdminOnly: sadece Admin rolü, ikisi de yoksa giriş yapmış herkes
public static class CategoryEndpoints
{
    public static void RegisterCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        // hepsi aynı route altında toplansın diye group kullandım
        var group = app.MapGroup("/categories").WithTags("Categories");

        group.MapGet(string.Empty, async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetCategoryQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetCategoryByIdQuery(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapPost(string.Empty, async (CreateCategoryCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapPut(string.Empty, async (UpdateCategoryCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        group.MapDelete("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new RemoveCategoryCommand(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}
