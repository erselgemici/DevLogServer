namespace DevLog.API.Endpoints.Registration
{
    public static class EndpointRegistrations
    {
        // bütün endpoint gruplarını tek yerden ekliyorum, Program.cs sade kalsın diye
        public static void RegisterEndpoints(this IEndpointRouteBuilder app)
        {
            app.RegisterCategoryEndpoints();
            app.RegisterBlogEndpoints();
            app.RegisterUserEndpoints();
            app.RegisterCommentEndpoints();
            app.RegisterSubCommentEndpoints();
            app.RegisterContactInfoEndpoints();
            app.RegisterMessageEndpoints();
            app.RegisterSocialEndpoints();
            app.RegisterTagEndpoints();
            app.RegisterDashboardEndpoints();
        }
    }
}
