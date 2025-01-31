using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations
{
    public partial class KitaplarSiraEklendi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sira",
                table: "Kitaplars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sira",
                table: "Kitaplars");
        }
    }
} 