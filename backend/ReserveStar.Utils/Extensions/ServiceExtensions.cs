using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using ReserveStar.Data;
using ReserveStar.Core.Authorization;
using ReserveStar.Helper;
using ReserveStar.Core.Data;
using ReserveStar.Core.Security;
using ReserveStar.Core.Resources;
using ReserveStar.Core.Cache;
using ReserveStar.Core.Queue;
using ReserveStar.Helper.Constants;

using FluentValidation;
using ReserveStar.Application.Auth.Commands.LoginCommand;

namespace ReserveStar.Utils.Extensions;

public static class ServiceExtensions
{
   public static IServiceCollection AddEnvironmentVariables(this IServiceCollection services)
   {
      var root = Directory.GetCurrentDirectory();
      var parent = Directory.GetParent(root);
      var filePath = Path.Combine(parent!.FullName, ".env");
      {
         if (File.Exists(filePath))
         {
            foreach (var line in File.ReadAllLines(filePath))
            {
               var parts = line.Split(
                   '=',
                   StringSplitOptions.RemoveEmptyEntries);

               if (parts.Length != 2)
                  continue;

               Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
         }
      }

      return services;
   }

   public static IServiceCollection AddCommonServices(this IServiceCollection services)
   {
      var applicationAssembly = typeof(LoginCommand).Assembly;

      services.AddHttpContextAccessor();
      services.AddOpenApi();

      services.AddMediatR(conf =>
      {
         conf.RegisterServicesFromAssembly(applicationAssembly);
      });

      services.AddValidatorsFromAssembly(applicationAssembly);

      services.AddCors(options =>
      {
         options.AddPolicy("LocalPolicy", policy =>
         {
            policy
               .WithOrigins(
                  "http://localhost:5173",
                  "https://localhost:5173",
                  "http://127.0.0.1:5173",
                  "https://[::1]:5173",
                  "http://localhost:5174",
                  "https://localhost:5174",
                  "http://127.0.0.1:5174",
                  "https://[::1]:5174")
               .WithExposedHeaders(MiddlewareConstants.RefreshedAccessTokenHeaderName)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
         });
      });

      services.AddAuthentication(opt =>
         {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(opt =>
            {
               opt.RequireHttpsMetadata = false;
               opt.SaveToken = true;
               opt.TokenValidationParameters = new TokenValidationParameters
               {
                  ValidateIssuer = true,
                  ValidIssuer = "computeneer",
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtTokenKey)),
                  ClockSkew = TimeSpan.Zero
               };
            });

      services.AddAuthorization();

      services.AddDbContext<DbContext, DataContext>();

      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
      services.AddScoped<ITransactionManager, EfTransactionManager>();
      services.AddScoped<IJwtManager, JwtManager>();
      services.AddScoped<IPasswordHasher, PasswordHasher>();
      services.AddScoped<IResourceManager, ResourceManager>();
      services.AddScoped<ICacheUnit, RedisCacheUnit>();
      services.AddScoped<ICacheManager, CacheManager>();
      services.AddScoped<IUserPermissionManager, UserPermissionManager>();
      services.AddScoped<IMessageTemplateManager, MessageTemplateManager>();
      services.AddScoped<IQueueManager, QueueManager>();

      services.AddSingleton<IQueueTransportManager, RabbitMqQueueTransportManager>();


      return services;
   }
}
