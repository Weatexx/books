using Microsoft.EntityFrameworkCore.Migrations;

public partial class IletisimTablosuGuncelle : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Önce eposta sütununu yeniden adlandır
        migrationBuilder.RenameColumn(
            name: "eposta",
            table: "iletisim",
            newName: "email");

        // AdSoyad sütununu ekle
        migrationBuilder.AddColumn<string>(
            name: "adsoyad",
            table: "iletisim",
            type: "varchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        // Diğer sütunların tiplerini güncelle
        migrationBuilder.AlterColumn<string>(
            name: "mesaj",
            table: "iletisim",
            type: "varchar(500)",
            maxLength: 500,
            nullable: false,
            oldType: "varchar(600)");

        migrationBuilder.AlterColumn<string>(
            name: "konu",
            table: "iletisim",
            type: "varchar(150)",
            maxLength: 150,
            nullable: false);

        migrationBuilder.AlterColumn<string>(
            name: "ip",
            table: "iletisim",
            type: "char(50)",
            maxLength: 50,
            nullable: false);

        migrationBuilder.AlterColumn<bool>(
            name: "goruldu",
            table: "iletisim",
            type: "tinyint(1)",
            nullable: false);

        migrationBuilder.AlterColumn<DateTime>(
            name: "tarihSaat",
            table: "iletisim",
            type: "datetime",
            nullable: false);
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

        // Diğer sütunları eski haline döndür
        migrationBuilder.AlterColumn<string>(
            name: "mesaj",
            table: "iletisim",
            type: "varchar(600)",
            maxLength: 600,
            nullable: false);
    }
} 