using Microsoft.EntityFrameworkCore.Migrations;

public partial class IletisimTablosuKolonAyarlari : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "mesaj",
            table: "iletisim",
            type: "varchar(500)",
            maxLength: 500,
            nullable: false,
            oldType: "longtext");

        migrationBuilder.AlterColumn<string>(
            name: "konu",
            table: "iletisim",
            type: "varchar(150)",
            maxLength: 150,
            nullable: false,
            oldType: "longtext");

        migrationBuilder.AlterColumn<string>(
            name: "ip",
            table: "iletisim",
            type: "char(50)",
            fixedLength: true,
            maxLength: 50,
            nullable: false,
            oldType: "longtext");

        migrationBuilder.AlterColumn<bool>(
            name: "goruldu",
            table: "iletisim",
            type: "tinyint(1)",
            nullable: false,
            oldType: "tinyint(1)");

        migrationBuilder.AlterColumn<string>(
            name: "email",
            table: "iletisim",
            type: "varchar(100)",
            maxLength: 100,
            nullable: false,
            oldType: "longtext");

        migrationBuilder.AlterColumn<string>(
            name: "adsoyad",
            table: "iletisim",
            type: "varchar(100)",
            maxLength: 100,
            nullable: false,
            oldType: "longtext");

        migrationBuilder.AlterColumn<DateTime>(
            name: "tarihSaat",
            table: "iletisim",
            type: "datetime",
            nullable: false,
            oldType: "datetime(6)");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Eski haline döndürmek için gerekli kodlar...
    }
} 