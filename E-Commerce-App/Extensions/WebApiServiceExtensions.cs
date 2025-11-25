using E_Commerce_App.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_App.Extensions
{
    public static class WebApiServiceExtensions
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory =
                ApiResponseFactory.CustomValidationErrorResponse;
            });
           services.AddSwaggerGen();
           services.AddEndpointsApiExplorer();
            services.AddCors(config =>

            config.AddPolicy("CROSPolicy", options =>
          options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")));
            return services;

        }

    }
}
