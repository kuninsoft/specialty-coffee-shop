using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpecialtyCoffeeShop.Models;
using SpecialtyCoffeeShop.Models.RequestDto;

namespace SpecialtyCoffeeShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IOptions<AdminCredentials> _credentialsOptions;

    public AuthController(IOptions<AdminCredentials> credentialsOptions)
    {
        ArgumentNullException.ThrowIfNull(credentialsOptions.Value.Username);
        ArgumentNullException.ThrowIfNull(credentialsOptions.Value.Password);
        
        _credentialsOptions = credentialsOptions;
    }
    
    // POST api/<AuthController>/Login
    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] AuthDto request)
    {
        if (!ModelState.IsValid
            || request.Username != _credentialsOptions.Value.Username
            || request.Password != _credentialsOptions.Value.Password)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, _credentialsOptions.Value.Username),
            new (ClaimTypes.Role, "Admin")
        };
            
        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return Ok();
    }

    // POST api/<AuthController>/Logout
    [HttpPost(nameof(Logout))]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return Ok();
    }
    
    // GET api/<AuthController>/Check
    [Authorize]
    [HttpGet(nameof(Check))]
    public IActionResult Check()
    {
        return Ok();
    }
}