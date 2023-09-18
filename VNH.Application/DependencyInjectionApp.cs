using Microsoft.Extensions.DependencyInjection;
using VNH.Application.Interfaces.Catalog.User;
using VNH.Application.Services.Catalog.User;

namespace VNH.Application
{
    public static class DependencyInjectionApp
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
