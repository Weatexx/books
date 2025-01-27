using System.ComponentModel.DataAnnotations;

namespace books.Models.ViewModels
{
    public class YazarlarVM
    {
        public int Id { get; set; }
        [Required]
        public string Adi { get; set; }
        [Required]
        public string Soyadi { get; set; }
        [Required]
        public DateTime DogumTarihi { get; set; }
        [Required]
        public string DogumYeri { get; set; }
        [Required]
        public bool Cinsiyeti { get; set; }
        

    }
}