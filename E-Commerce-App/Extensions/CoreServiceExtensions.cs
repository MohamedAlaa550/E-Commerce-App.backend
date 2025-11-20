using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;
using Shared;

namespace E_Commerce_App.Extensions
{
    public static class CoreServiceExtensions
    {
        public static IServiceCollection AddCoreServices (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceManger, ServiceManger>();
            services.AddAutoMapper(o => { }, typeof(AssemblyReference).Assembly);
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
    }
}
