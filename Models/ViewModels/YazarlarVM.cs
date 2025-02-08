using System.ComponentModel.DataAnnotations;

namespace books.Models.ViewModels
{
    public class YazarlarVM
    {
        public int ID { get; set; }
        [Required]
        public string adi { get; set; } = "";
        [Required]
        public string   soyadi { get; set; } = "";
        [Required]
        public DateTime dogumTarihi { get; set; }
        [Required]
        public string dogumYeri { get; set; } = "";
        [Required]
        public string cinsiyeti { get; set; } = "E";
        public int KitapSayisi { get; set; }
    }
}