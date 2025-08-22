using Microsoft.AspNetCore.Identity;

namespace ApiCatalogo.Model;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
