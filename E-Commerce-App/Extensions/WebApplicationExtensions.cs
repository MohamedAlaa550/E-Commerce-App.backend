using Domain.Contracts;
using E_Commerce_App.MidlleWarers;

namespace E_Commerce_App.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();
            return app;

        }

        public static WebApplication UseCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleWare>();
            return app;
        }

        public static WebApplication UseSwaggerMiddleware(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }

    }
}
