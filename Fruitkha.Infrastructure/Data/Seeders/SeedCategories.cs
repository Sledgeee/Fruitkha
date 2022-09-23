using Fruitkha.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fruitkha.Infrastructure.Data.Seeders;

public static class SeedCategories
{
    public static void Seed(this ModelBuilder builder)
    {
        builder.Entity<Category>().HasData(
            new Category()
            {
                Id = 1000,
                ParentId = 1,
                RealCategory = 0,
                Name = "Computers and notebooks"
            },
            new Category()
            {
                Id = 1001,
                ParentId = 1,
                RealCategory = 0,
                Name = "Mobile phones"
            }
        );
    }
}