﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using VNH.Application.Interfaces.Common;
using VNH.Infrastructure.Implement.Common;
using VNH.Infrastructure.Implement.Catalog.Account;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using VNH.Application.Interfaces.Catalog.Accounts;
using VNH.Application.Interfaces.Catalog.Topics;
using VNH.Infrastructure.Implement.Catalog.Topics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using VNH.Application.Interfaces.Posts;
using VNH.Infrastructure.Implement.Catalog.Posts;
using VNH.Application.Interfaces.Catalog.HashTags;
using VNH.Infrastructure.Implement.Catalog.HashTags;

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

 
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = "/User/Login";
                     options.LogoutPath = "/User/Signup";
                     options.AccessDeniedPath = "/User/Forbidden/";

                     options.CookieManager = new ChunkingCookieManager();

                     options.Cookie.HttpOnly = true;
                     options.Cookie.SameSite = SameSiteMode.None;
                     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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
                //// googleOptions.CallbackPath = "/signin-google";
                ////googleOptions.AccessDeniedPath = "/Login";
                ////googleOptions.SaveTokens = true;
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = configuration.GetValue<string>("Authentication:Facebook:AppId");
                facebookOptions.AppSecret = configuration.GetValue<string>("Authentication:Facebook:AppSecret");
               //// facebookOptions.CallbackPath = "/FacebookCallback";
               // //facebookOptions.SaveTokens = true;

            });
            services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDev", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
            services.AddOptions();
            services.AddSession();
            var mailsettings = configuration.GetSection("MailSettings");  
            services.Configure<MailSettings>(mailsettings);
            services.AddSingleton<ISendMailService, SendMailService>();

            services.AddSingleton<IImageService, ImageService>();

            services.AddScoped<IHashTag, TagService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IPostService, PostService>();
            

            

            return services;
        }
    }
}
