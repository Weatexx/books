namespace books.Models.ViewModels;
public class TurListVM
{
    [System.ComponentModel.DataAnnotations.Required]
    public int Id { get; set; }
    [System.ComponentModel.DataAnnotations.Required]
    public string TurAdi { get; set; }
}