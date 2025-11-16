
using Domain.Contracts;
using E_Commerce_App.MidlleWarers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Data;
using Persistence.Data.Context;
using Persistence.Repositories;
using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;
using System.Threading.Tasks;

namespace E_Commerce_App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Services Container
            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>((OptionsBuilder) =>
            {
                OptionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); 

            builder.Services.AddScoped<IDbInitializer,DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManger, ServiceManger>();
            builder.Services.AddAutoMapper(o => { },typeof(AssemblyReference).Assembly);



            #endregion

            var app = builder.Build();
            await InitializeDatabaseAsync(app);

            #region Middlewares
            // Configure the HTTP request pipeline.
            app.UseMiddleware<GlobalErrorHandlingMiddleWare>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
            async Task InitializeDatabaseAsync(WebApplication app)
            {
                using var scope = app.Services.CreateScope();
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await dbInitializer.InitializeAsync();
            }
        }
    }
}
