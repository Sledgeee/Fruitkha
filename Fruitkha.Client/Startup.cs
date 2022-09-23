using Fruitkha.Client.Jobs;
using Fruitkha.Client.Middlewares;
using Microsoft.AspNetCore.Identity;
using Fruitkha.Client.ServiceExtensions;
using Fruitkha.Core;
using Fruitkha.Infrastructure;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;
using React.AspNet;

namespace Fruitkha.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName).AddV8();
            services.AddControllersWithViews();
            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.AddIdentityDbContext();
            services.AddAuthentication();

            services.AddRepositories();
            services.AddCustomServices();
            services.ConfigureJwtOptions(Configuration.GetSection("JwtOptions"));
            services.ConfigureMailSettings(Configuration);
            services.ConfigureValidationSettings(Configuration);
            services.ConfigureImageSettings(Configuration);
            services.AddAutoMapper();
            services.AddPolicyServices();
            services.AddJwtAuthentication(Configuration);
            services.AddCors();
            services.AddMvcCore().AddRazorViewEngine();
  
            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = null;
            });

            services.AddCronJob<MyJob>(cron =>
            {
                cron.TimeZoneInfo = TimeZoneInfo.Local;
                cron.CronExpression = @"*/1 * * * *";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/error";
                    await next();
                }
            });

            app.UseReact(config =>
            {

            });

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseCors(c =>
            {
                c.AllowAnyOrigin();
                c.AllowAnyHeader();
                c.AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
