using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration) => _configuration = configuration;

    public sealed record TokenRequest(string Username, string Role = "Customer");

    [HttpPost("token")]
    public IActionResult GenerateToken(TokenRequest request)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "dev-only-secret-key-change-in-production-min-32-chars";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, request.Role)
        };

        var token = new JwtSecurityToken(
            issuer: "OrderService.Api",
            audience: "OrderService.Api.Clients",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return Ok(new { access_token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
