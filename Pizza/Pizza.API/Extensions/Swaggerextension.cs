using Microsoft.OpenApi.Models;

namespace Pizza.API.Extensions
{
    public static class Swaggerextension
    {
        public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // General Swagger configuration
                options.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "My Swagger",
                    Version = "v1",
                });

                // Configuration related to using JWT token directly in the interface
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder AddSwaggerExtended(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/V1/swagger.json", "My Swagger Api");
                options.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
