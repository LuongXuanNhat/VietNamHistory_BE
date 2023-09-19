using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VNH.Application.Services.Catalog.Users;
using VNH.Domain;
using Microsoft.Extensions.Configuration;
using VNH.Infrastructure.Implement.Catalog.Users;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure
{
    public static class DependencyInjectionInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<IUserService, UserService>();
            services.AddDbContext<VietNamHistoryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DataConnect"));
            });
            services.AddIdentity<User,Role>()
            .AddEntityFrameworkStores<VietNamHistoryContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
