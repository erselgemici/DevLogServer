using System.Security.Claims;
using MediatR;
using DevLog.Application.Constants;
using DevLog.Application.Features.Blogs.Commands;
using DevLog.Application.Features.Blogs.Queries;

namespace DevLog.API.Endpoints;

// minimal api endpointleri, controller yerine bu yapıyı kullandım
// bloglar herkese açık okunuyor, yazma işlemleri giriş yapmış yazar/admin için
public static class BlogEndpoints
{
    public static void RegisterBlogEndpoints(this IEndpointRouteBuilder app)
    {
        // hepsi aynı route altında toplansın diye group kullandım
        var group = app.MapGroup("/blogs").WithTags("Blogs");

        group.MapGet(string.Empty, async (IMediator mediator) =>
        {
            // endpoint sadece isteği MediatR'a gönderiyor, asıl iş handler'da
            var response = await mediator.Send(new GetBlogsQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous(); // giriş yapmadan da görülebilsin

        group.MapGet("{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetBlogByIdQuery(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapPost(string.Empty, async (CreateBlogCommand command, ClaimsPrincipal user, IMediator mediator) =>
        {
            // yazarı client'tan gelen değere değil token'a göre belirliyorum, başkası adına yazı eklenemesin
            command.UserId = user.FindFirstValue("sub")!;
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        });

        group.MapPut(string.Empty, async (UpdateBlogCommand command, ClaimsPrincipal user, IMediator mediator) =>
        {
            // handler'da "bu yazı bu kullanıcının mı" kontrolü için
            command.CurrentUserId = user.FindFirstValue("sub");
            command.IsAdmin = user.IsInRole(Roles.Admin);
            command.UserId = command.CurrentUserId!;
            var response = await mediator.Send(command);
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        });

        // id:guid ile sadece geçerli guid gelirse bu route çalışıyor
        group.MapDelete("{id:guid}", async (Guid id, ClaimsPrincipal user, IMediator mediator) =>
        {
            var response = await mediator.Send(new RemoveBlogCommand(id, user.FindFirstValue("sub"), user.IsInRole(Roles.Admin)));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        });

        group.MapGet("Latest5Blogs", async (IMediator mediator) =>
        {
            var response = await mediator.Send(new GetLatest5BlogsQuery());
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        // bir konudaki (etiketteki) yazılar: /api/blogs/byTag/dotnet
        group.MapGet("byTag/{slug}", async (string slug, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetBlogsByTagQuery(slug));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        // detay sayfasının altındaki "bu konudaki diğer yazılar"
        group.MapGet("{id:guid}/related", async (Guid id, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetRelatedBlogsQuery(id));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        // arama: /api/blogs/search?q=angular
        // q parametresi query string'den geliyor, [FromQuery] yazmasam da minimal api isimden buluyor
        group.MapGet("search", async (string q, IMediator mediator) =>
        {
            var response = await mediator.Send(new SearchBlogsQuery(q));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();

        group.MapGet("byCategoryId/{categoryId:guid}", async (Guid categoryId, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetBlogsByCategoryIdQuery(categoryId));
            return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
        }).AllowAnonymous();
    }
}
