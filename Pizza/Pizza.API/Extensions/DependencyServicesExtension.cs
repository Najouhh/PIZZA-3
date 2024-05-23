using Pizza.Application.Core.Interfaces;
using Pizza.Application.Core.Services;
using Pizza.Infrastructure.Repository.Interfaces;
using Pizza.Infrastructure.Repository.Repos;

namespace Pizza.API.Extensions
{
    public static class DependencyServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Alla mina repositoriaries
            services.AddScoped<IDishRepo, DishRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();

           // alla mina services
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
