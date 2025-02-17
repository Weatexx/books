namespace books.Models.ViewModels;

public class MesajViewModel
{
    public int Id { get; set; }
    public int GonderenId { get; set; }
    public string GonderenAdi { get; set; } = "";
    public string GonderenResim { get; set; } = "";
    public string Mesaj { get; set; } = "";
    public DateTime Tarih { get; set; }
    public bool Okundu { get; set; }
}

public class MesajGonderViewModel
{
    public int AliciId { get; set; }
    public string Mesaj { get; set; } = "";
} 