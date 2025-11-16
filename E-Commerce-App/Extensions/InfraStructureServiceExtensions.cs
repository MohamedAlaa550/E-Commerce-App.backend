using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Data.Context;
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

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect
            (configuration.GetConnectionString("RedisConnection")!));





            return services;

        
        }

    }
}
