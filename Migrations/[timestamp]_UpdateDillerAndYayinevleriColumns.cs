using Microsoft.EntityFrameworkCore.Migrations;

namespace books.Migrations
{
    public partial class UpdateDillerAndYayinevleriColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Diller tablosunu güncelle
            migrationBuilder.RenameColumn(
                name: "diladi",
                table: "diller",
                newName: "adi");

            migrationBuilder.AddColumn<string>(
                name: "kod",
                table: "diller",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);

            // Yayinevleri tablosunu güncelle
            migrationBuilder.RenameColumn(
                name: "yayineviadi",
                table: "yayinevleri",
                newName: "adi");

            migrationBuilder.RenameColumn(
                name: "tel",
                table: "yayinevleri",
                newName: "telefon");

            migrationBuilder.AddColumn<string>(
                name: "website",
                table: "yayinevleri",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "adres",
                table: "yayinevleri",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "telefon",
                table: "yayinevleri",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alma işlemleri...
        }
    }
} 