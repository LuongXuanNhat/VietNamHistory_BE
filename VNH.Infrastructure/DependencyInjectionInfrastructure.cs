using Microsoft.AspNetCore.Identity;
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
using VNH.Application.Interfaces.Catalog.Reports;
using VNH.Infrastructure.Implement.Catalog.Reports;
using VNH.Application.Interfaces.Documents;
using VNH.Infrastructure.Implement.Catalog.Documents;
using VNH.Application.Interfaces.Catalog.Forum;
using VNH.Infrastructure.Implement.Catalog.Forum;
using VNH.Application.Interfaces.Catalog.MultipleChoices;
using VNH.Infrastructure.Implement.Catalog.MultipleChoices;
using VNH.Application.Interfaces.Catalog.ExamHistory;
using VNH.Infrastructure.Implement.Catalog.ExamHistorys;
using VNH.Application.Interfaces.Catalog.NewsHistory;
using VNH.Infrastructure.Implement.Catalog.NewsHistory;
using VNH.Application.Implement.Catalog.NotificationServices;
using VNH.Infrastructure.Implement.Catalog.NotificationServices;

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
            services.AddIdentity<User,Role>()
            .AddEntityFrameworkStores<VietNamHistoryContext>()
            .AddDefaultTokenProviders();

 
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = "/UserShort/Login";
                     options.LogoutPath = "/UserShort/Signup";
                     options.AccessDeniedPath = "/UserShort/Forbidden/";

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
               ////facebookOptions.SaveTokens = true;

            });
            services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
            });
            
            services.AddOptions();
            services.AddSession();
            var mailsettings = configuration.GetSection("MailSettings");  
            services.Configure<MailSettings>(mailsettings);
            services.AddSingleton<ISendMailService, SendMailService>();
            services.AddSingleton<IStorageService, StorageService>();

            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IHashTag, TagService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IMultipleChoiceService, MultipleChoiceService>();
            services.AddScoped<IExamHistoryService, ExamHistoryService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSignalR();

            


            return services;
        }
    }
}
