using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fruitkha.Infrastructure.Migrations
{
    public partial class SeedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentId", "RealCategory" },
                values: new object[,]
                {
                    { 1000, "Computers and notebooks", 1, 0 },
                    { 1001, "Mobile phones", 1, 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1001);
        }
    }
}
