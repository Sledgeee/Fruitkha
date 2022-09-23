using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Fruitkha.Core.Helpers;
using Fruitkha.Core.Helpers.Cron;
using Fruitkha.Core.Interfaces;
using Fruitkha.Core.Services;

namespace Fruitkha.Client.ServiceExtensions;

public static class ServiceExtension
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            };

            cfg.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                        context.Response.Headers.Add("Access-Control-Expose-Headers", "Token-Expired");
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void AddPolicyServices(this IServiceCollection services)
    {
        //services.AddTransient<IAuthorizationPolicyProvider, GroupChatRolesPolicyProvider>();
        //services.AddTransient<IAuthorizationHandler, GroupChatRolesAuthorizationHandler>();
    }

    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options)
        where T : CronJobService
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options), @"Please, provide Schedule Configurations");
        }

        var config = new ScheduleConfig<T>();
        options.Invoke(config);
        if (string.IsNullOrWhiteSpace(config.CronExpression))
        {
            throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression),
                @"Empty Cron Expression is not allowed");
        }

        services.AddSingleton<IScheduleConfig<T>>(config);
        services.AddHostedService<T>();
        return services;
    }
}