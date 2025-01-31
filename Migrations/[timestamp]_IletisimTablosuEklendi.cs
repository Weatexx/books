using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

public partial class IletisimTablosuEklendi : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "iletisim",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Eposta = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                Konu = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                Mesaj = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: false),
                TarihSaat = table.Column<DateTime>(type: "datetime", nullable: false),
                Ip = table.Column<string>(type: "char(50)", maxLength: 50, nullable: false),
                Goruldu = table.Column<byte>(type: "tinyint(1)", nullable: false, defaultValue: 0)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_iletisim", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "iletisim");
    }
} 