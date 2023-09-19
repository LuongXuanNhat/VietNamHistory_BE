using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VNH.Application.Services.Catalog.Users;
using VNH.Domain;
using VNH.Infrastructure.Implement.Catalog.Users;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure
{
    public static class DependencyInjectionInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IUserService, UserService>();
            services.AddDbContext<VietNamHistoryContext>();
            services.AddIdentity<User,Role>()
            .AddEntityFrameworkStores<VietNamHistoryContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
