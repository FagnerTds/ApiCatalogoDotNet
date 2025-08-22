using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTO;

public class LoginModel
{
    [Required(ErrorMessage ="User name is Required")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "User name is Required")]
    public string? Password { get; set; }

}
