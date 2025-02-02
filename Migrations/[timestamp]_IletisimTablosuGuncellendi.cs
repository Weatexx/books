using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations
{
    public partial class IletisimTablosuGuncellendi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "eposta",
                table: "iletisim",
                newName: "email");

            migrationBuilder.AddColumn<string>(
                name: "adsoyad",
                table: "iletisim",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adsoyad",
                table: "iletisim");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "iletisim",
                newName: "eposta");
        }
    }
} 