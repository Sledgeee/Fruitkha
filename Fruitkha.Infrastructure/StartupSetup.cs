using Fruitkha.Core.Entities;
using Fruitkha.Core.Interfaces;
using Fruitkha.Infrastructure.Data.DbContexts;
using Fruitkha.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fruitkha.Infrastructure
{
    public static class StartupSetup
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        }

        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<VelarDbContext>(x => x.UseNpgsql(connectionString));
        }

        public static void AddIdentityDbContext(this IServiceCollection services)
        {
            services.AddIdentity<User,
                IdentityRole>().AddEntityFrameworkStores<VelarDbContext>().AddDefaultTokenProviders();
        }
    }
}