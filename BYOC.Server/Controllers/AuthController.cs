using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BYOC.Server.Models;
using BYOC.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BYOC.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(
        IConfiguration configuration,
        IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromQuery] string apiKey, CancellationToken token = default)
    {
        var user = await _userService.FindByApiKey(apiKey, token);
        if (user is not null)
        {
            var claims = await _userService.GetClaimsAsync(user, token);
            
            //create a signing secret
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signinCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);  
            //create token options
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:TokenExpirationInMinutes"])),
                claims: claims,
                signingCredentials: signinCredentials
            );
            //create token
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            //return token
            return new OkObjectResult(new AuthToken {JwtToken = tokenString});

        }

        return Unauthorized();
    }
}