using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Data.Context;
using Persistence.Identity;
using Persistence.Repositories;
using Services.Abstraction.Contracts;
using Shared;
using StackExchange.Redis;
using System.Text;

namespace E_Commerce_App.Extensions
{
    public static class InfraStructureServiceExtensions
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services
            , IConfiguration configuration)
        {

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheRepository, CacheRepository>();
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

            services.ConfigureJwt(configuration);





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

        public static IServiceCollection ConfigureJwt(this IServiceCollection services
            , IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.AddAuthentication(options => { 
            options.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime =  true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                };
                });
            services.AddAuthentication();
            return services;
        }
          
    }
}
