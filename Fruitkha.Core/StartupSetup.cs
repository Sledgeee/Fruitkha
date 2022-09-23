using AutoMapper;
using Fruitkha.Core.Helpers;
using Fruitkha.Core.Helpers.Mails;
using Fruitkha.Core.Interfaces;
using Fruitkha.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fruitkha.Core
{
    public static class StartupSetup
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IConfirmEmailService, ConfirmEmailService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<ICatalogService, CatalogService>();
        }

        public static void ConfigureJwtOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration);
        }

        public static void ConfigureValidationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //var configItem =
            //    configuration.GetSection("RolesAccess")
            //        .Get<Dictionary<GroupChatRoles, List<GroupChatRoles>>>();
            //services.AddSingleton<RoleAccess>(new RoleAccess() { RolesAccess = configItem });
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigureImageSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ImageSettings>(configuration.GetSection("ImageSettings"));
        }



        public static void ConfigureMailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("EmailSettings"));
        }
    }
}