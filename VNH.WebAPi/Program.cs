using VNH.Application;
using VNH.Infrastructure;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using VNH.Infrastructure.Presenters;
using Microsoft.Extensions.Configuration;
using VNH.Infrastructure.Presenters.Migrations;
using Microsoft.EntityFrameworkCore;
using VNH.Application.Interfaces.Catalog.Forum;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace VNH.WebAPi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Environment.IsDevelopment()
                                 ? builder.Configuration.GetConnectionString("LocalDataConnect")
                                 : builder.Configuration.GetConnectionString("DataConnect");

           // var connectionString = builder.Configuration.GetConnectionString("DataConnect");

            builder.Services.AddDbContext<VietNamHistoryContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            // Add DI


            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);

            });
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();


            #region  SwaggerGen Configuration
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger VietNamHistory Solution", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. <p>
                    Enter 'Bearer' [space] and then your token in the text input below.
                    </p> Example: Email: nam@gmail.com </br> <strong>Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJuYW1AZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoic3R1ZGVudCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJuYW1AZ21haWwuY29tIiwiZXhwIjoxNjk1NDQ2NDczLCJpc3MiOiJodHRwczovL3dlYmFwaS50ZWR1LmNvbS52biIsImF1ZCI6Imh0dHBzOi8vd2ViYXBpLnRlZHUuY29tLnZuIn0.l6gsRiDGBQwXl2UQEzY7asEGlvQyIJAg4PQFh22fViw</strong>",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "Oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            
                        },
                        new List<string>()
                    }
                });
            });
            #endregion

            #region JWT Authentication Configuration
            string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
            string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = System.TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });



            #endregion

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDev", builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://luongxuannhat.github.io", "https://toiyeulichsu.com")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpLogging();

            app.Use(async (context, next) =>
            {
                app.Logger.LogInformation("Request RemoteIp: {RemoteIpAddress}",
                    context.Connection.RemoteIpAddress);
                await next(context);
            });
            
            app.UseStaticFiles();
            app.UseForwardedHeaders();
            app.UseSession();
            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseCors("AllowAngularDev");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatSignalR>("/commentHub");
            });
            app.Run();
        }


    }
}