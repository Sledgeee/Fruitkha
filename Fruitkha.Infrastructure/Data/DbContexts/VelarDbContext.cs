using Fruitkha.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fruitkha.Infrastructure.Data.Seeders;

namespace Fruitkha.Infrastructure.Data.DbContexts
{
    public class VelarDbContext : IdentityDbContext<User>
    {
        public VelarDbContext(DbContextOptions<VelarDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Seed();
            base.OnModelCreating(builder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}