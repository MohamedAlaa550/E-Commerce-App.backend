using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Data.Context;
using Persistence.Identity;
using Persistence.Repositories;
using StackExchange.Redis;

namespace E_Commerce_App.Extensions
{
    public static class InfraStructureServiceExtensions
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services
            , IConfiguration configuration)
        {

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<StoreDbContext>((OptionsBuilder) =>
            {
                OptionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityContext>((OptionsBuilder) =>
            {
                OptionsBuilder.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.ConfigureIdentityServices();

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect
            (configuration.GetConnectionString("RedisConnection")!));





            return services;


        }

        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services
           )
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<StoreIdentityContext>();
            return services;
        }
    }
}
