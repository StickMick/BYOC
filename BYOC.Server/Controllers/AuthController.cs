using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BYOC.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BYOC.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromQuery] string apiKey)
    {
        var appkeyexists = true;
        if(appkeyexists){       
            //create claims list
            List<Claim> authClaims = new List<Claim>();
            authClaims.Add(new Claim("apiKey",apiKey,ClaimValueTypes.String));
            authClaims.Add(new Claim("role","filler",ClaimValueTypes.String));
            authClaims.Add(new Claim("email", "test@example.com", ClaimValueTypes.String));

            //create a signing secret
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signinCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);  
            //create token options
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:TokenExpirationInMinutes"])),
                claims: authClaims,
                signingCredentials: signinCredentials
            );
            //create token
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            //return token
            return new OkObjectResult(new AuthToken {JwtToken = tokenString});

        } else {
            return Unauthorized();
        }
    }
}