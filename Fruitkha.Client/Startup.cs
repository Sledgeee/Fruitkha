using Fruitkha.Client.Jobs;
using Fruitkha.Client.Middlewares;
using Microsoft.AspNetCore.Identity;
using Fruitkha.Client.ServiceExtensions;
using Fruitkha.Core;
using Fruitkha.Infrastructure;
using Fruitkha.Services.Emailer;

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
            services.AddControllersWithViews();
            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.AddIdentityDbContext();
            services.AddAuthentication();

            services.AddScoped<IEmailSender, EmailSender>();

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
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/error");

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
