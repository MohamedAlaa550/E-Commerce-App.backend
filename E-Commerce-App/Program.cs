
using Domain.Contracts;
using E_Commerce_App.Extensions;
using E_Commerce_App.Factories;
using E_Commerce_App.MidlleWarers;
using Microsoft.AspNetCore.Mvc;
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
            
   
           builder.Services.AddInfraStructureServices(builder.Configuration);
           builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddWebApiServices();



            #endregion

            var app = builder.Build();
            await app.SeedDbAsync();

            #region Middlewares
            // Configure the HTTP request pipeline.
            app.UseCustomExceptionMiddleware();
            if (app.Environment.IsDevelopment())
            {
             app.UseSwaggerMiddleware();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CROSPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
          
        }
    }
}
