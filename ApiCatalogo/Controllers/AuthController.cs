using ApiCatalogo.DTO;
using ApiCatalogo.Model;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiCatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = "SuperAdminOnly")]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);

        
        if (!roleExist)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (roleResult.Succeeded)
            {
                _logger.LogInformation(1, "Roles Added");
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Seccess", Message =
                    $"Role {roleName} added successfully"});
            }
            else
            {
                _logger.LogInformation(2, "Error");
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message =
                    $"Issue Addinng the new {roleName} role" });
            }
        }
        return StatusCode(StatusCodes.Status400BadRequest,
            new Response { Status = "Error", Message = "Role already exist." });
    }

    [HttpPost]
    [Authorize(Policy = "SuperAdminOnly")]
    [Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return BadRequest(new { error = "User not found" });

        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
            return BadRequest(new { error = $"Role {roleName} does not exists" });

        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (result.Succeeded)
        {
            _logger.LogInformation($"User {user.Email} added to the {roleName} role");
            return Ok(new Response { Status = "Success", Message = $"User {user.Email} added to the {roleName} role" });
        }
        else
        {
            _logger.LogInformation($"Error: Unable to add user {user} to the {roleName} role");
            return StatusCode(StatusCodes.Status400BadRequest, new Response
            {
                Status = "Error",
                Message =
                $"Error: Unable to add user {user} to the {roleName} role"
            });
            
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName !);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("id", user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                                    out int refreshTokenValidityInMinutes);

            user.RefreshTokenExpiryTime = 
                DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);
            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.UserName!);

        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User already exists!" });
        }

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User creation failed." });
        }

        return Ok(new Response { Status = "Seccess", Message = "User created successfully!" });

    }

    [HttpPost]
    [Route("Refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid Client request");
        }

        string? accessToken = tokenModel.AccessToken
                                ?? throw new ArgumentNullException(nameof(tokenModel));

        string? refreshToken = tokenModel.RefreshToken
                                ?? throw new ArgumentException(nameof(tokenModel));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        string userName = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(userName!);

        if (user == null || user.RefreshToken != refreshToken 
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });

    }

    [HttpPost]
    [Authorize(Policy = "ExclusiveOnly")]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
            return BadRequest("Ivalid User name");
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }    

}
