using Microsoft.EntityFrameworkCore.Migrations;

namespace books.Migrations
{
    public partial class YazarlarBiyografiKaldir : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biyografi",
                table: "Yazarlars");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biyografi",
                table: "Yazarlars",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
} 