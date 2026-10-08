using MediatR;
using DevLog.Application.Features.Dashboard.Queries;

namespace DevLog.API.Endpoints;

// admin paneli özet ekranı, giriş yapmış herkes (admin ve yazar) görebiliyor
// AllowAnonymous yazmadığım için /api grubundaki RequireAuthorization burada da geçerli
public static class DashboardEndpoints
{
    public static void RegisterDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/dashboard").WithTags("Dashboard");

        group.MapGet(string.Empty, async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetDashboardQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        });
    }
}
