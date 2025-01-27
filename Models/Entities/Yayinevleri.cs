using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace books.Models.Entities;

public partial class Yayinevleri
{
    public int Id { get; set; }

    [Required]
    public string? yayineviAdi { get; set; }

    public string? adres { get; set; }

    public string? tel { get; set; }

    public int sira { get; set; }
}
