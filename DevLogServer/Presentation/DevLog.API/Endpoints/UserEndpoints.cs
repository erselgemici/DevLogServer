using System.Security.Claims;
using MediatR;
using DevLog.Application.Constants;
using DevLog.Application.Features.Users.Commands;
using DevLog.Application.Features.Users.Queries;

namespace DevLog.API.Endpoints;

public static class UserEndpoints
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("/users").WithTags("Users");

        // kayıt ve giriş token olmadan çağrılabilmeli
        users.MapPost("register", async (CreateUserCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        users.MapPost("login", async (GetLoginQuery query, IMediator mediator) =>
        {
            var response = await mediator.Send(query);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        // kullanıcı listesi ve rol değiştirme sadece admin için
        users.MapGet(string.Empty, async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetUsersQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);

        users.MapPut("role", async (ChangeUserRoleCommand command, ClaimsPrincipal user, IMediator mediator) =>
        {
            command.CurrentUserId = user.FindFirstValue("sub");
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).RequireAuthorization(Policies.AdminOnly);
    }
}
