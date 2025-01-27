using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace books.Models.Entities;

public partial class Yazarlar
{
    public int Id { get; set; }

    [Required]
    public string? Adi { get; set; }

    [Required]
    public string? Soyadi { get; set; }

    public DateTime DogumTarihi { get; set; }

    public string? DogumYeri { get; set; }

    public bool Cinsiyeti { get; set; }
}
