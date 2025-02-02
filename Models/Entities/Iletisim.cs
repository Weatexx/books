using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

public class Iletisim
{
    [Column("id")]
    public int Id { get; set; }

    [Column("adsoyad")]
    public string AdSoyad { get; set; } = "";

    [Column("eposta")]
    public string Email { get; set; } = "";

    [Column("konu")]
    public string Konu { get; set; } = "";

    [Column("mesaj")]
    public string Mesaj { get; set; } = "";

    [Column("tarihSaat")]
    public DateTime TarihSaat { get; set; }

    [Column("ip")]
    public string Ip { get; set; } = "";

    [Column("goruldu")]
    public bool Goruldu { get; set; }
}
