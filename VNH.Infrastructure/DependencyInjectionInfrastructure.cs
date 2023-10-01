using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VNH.Application.Services.Catalog.Users;
using VNH.Domain;
using VNH.Infrastructure.Implement.Catalog.Users;
using VNH.Infrastructure.Presenters.Migrations;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using VNH.Application.DTOs.Common.SendEmail;
using VNH.Application.Interfaces.Email;
using VNH.Infrastructure.Presenters.Email;
using AutoMapper;
using VNH.Application.Interfaces.Common;
using VNH.Infrastructure.Implement.Common;
using VNH.Application.Interfaces.Catalog.IAccountService;
using VNH.Infrastructure.Implement.Catalog.Account;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace VNH.Infrastructure
{
    public static class DependencyInjectionInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddLogging();
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .CreateLogger();

            services.AddScoped<IUserService, UserService>();
            services.AddDbContext<VietNamHistoryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DataConnect"));
            });
            services.AddIdentity<User,Role>()
            .AddEntityFrameworkStores<VietNamHistoryContext>()
            .AddDefaultTokenProviders();

            // Mail settings
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = "/User/Login";
                     options.LogoutPath = "/User/Signup";
                     options.AccessDeniedPath = "/User/Forbidden/";
                 });
            // Identity settings
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = false;
            });

            // Facebook, Google
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });
            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = configuration.GetValue<string>("Authentication:Google:AppId");
                    googleOptions.ClientSecret = configuration.GetValue<string>("Authentication:Google:AppSecret");
                    //googleOptions.CallbackPath = "/Home";
                    //googleOptions.AccessDeniedPath = "/Login";
                    //googleOptions.SaveTokens = true;
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = configuration.GetValue<string>("Authentication:Facebook:AppId");
                    facebookOptions.AppSecret = configuration.GetValue<string>("Authentication:Facebook:AppSecret");
                    //facebookOptions.CallbackPath = "/Home";
                    //facebookOptions.AccessDeniedPath = "/Login";
                    //facebookOptions.SaveTokens = true;

                });


            services.AddOptions();                                         
            var mailsettings = configuration.GetSection("MailSettings");  
            services.Configure<MailSettings>(mailsettings);
            services.AddSingleton<ISendMailService, SendMailService>();

            services.AddSingleton<IImageService, ImageService>();
            services.AddScoped<IAccountService, AccountService>();


            return services;
        }
    }
}
