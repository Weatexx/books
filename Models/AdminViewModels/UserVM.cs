using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class UserVM
{
    public int id { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
    [MinLength(3, ErrorMessage = "Kullanıcı adı en az 3 karakter olmalıdır!")]
    public string username { get; set; } = "";

    [Required(ErrorMessage = "Şifre zorunludur")]
    public string password { get; set; } = "";
}