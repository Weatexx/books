using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class UsersVM
{
    public int id { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
    public string username { get; set; } = "";

    [Required(ErrorMessage = "Şifre zorunludur")]
    public string password { get; set; } = "";

    public UsersVM()
    {
        username = "";
        password = "";
    }
}