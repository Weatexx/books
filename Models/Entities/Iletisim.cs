using System;
using System.Collections.Generic;

namespace books.Models.Entities;

public class Iletisim
{
    public int Id { get; set; }

    public string Eposta { get; set; } = "";

    public string Konu { get; set; } = "";

    public string Mesaj { get; set; } = "";

    public DateTime TarihSaat { get; set; }

    public string Ip { get; set; } = "";

    public bool Goruldu { get; set; }
}
