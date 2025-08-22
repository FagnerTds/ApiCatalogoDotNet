using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTO;

public class RegisterModel
{
    [Required(ErrorMessage = "User name is Required")]
    public string? UserName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "User name is Required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "User name is Required")]
    public string? Password { get; set; }
}
