using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class UsersVM
{
    public int id { get; set; }

    [Required]
    public string username { get; set; }
    [Required] 
    public string password { get; set; }
}