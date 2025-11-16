using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;

namespace E_Commerce_App.Extensions
{
    public static class CoreServiceExtensions
    {
        public static IServiceCollection AddCoreServices (this IServiceCollection services)
        {
            services.AddScoped<IServiceManger, ServiceManger>();
            services.AddAutoMapper(o => { }, typeof(AssemblyReference).Assembly);
            return services;
        }
    }
}
