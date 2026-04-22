using System.Security.Cryptography;
using System.Text;
using EmployeePortal.Data;
using EmployeePortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePortal.Controllers;

public record LoginRequest(string Username, string Password);

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly PortalDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(PortalDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null)
            {
                return Unauthorized(new { error = "User not found" });
            }

            var hash = ComputeMd5(request.Password);

            if (hash != user.PasswordHash)
            {
                return Unauthorized(new { error = "Invalid password" });
            }

            user.LastLogin = DateTime.UtcNow;
            _db.SaveChanges();

            var token = GenerateToken(user);
            return Ok(new { token, role = user.Role });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
        }
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] LoginRequest request)
    {
        var existing = _db.Users.FirstOrDefault(u => u.Username == request.Username);
        if (existing != null) return BadRequest("Username already taken");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = ComputeMd5(request.Password),
            Role = "USER"
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        return Ok(new { user.Id, user.Username });
    }

    private static string ComputeMd5(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = MD5.HashData(bytes);
        return Convert.ToHexString(hash);
    }

    private string GenerateToken(User user)
    {
        var secret = _config["Jwt:Secret"] ?? "dev-fallback-secret-do-not-use";
        var payload = $"{user.Id}:{user.Username}:{user.Role}:{DateTime.UtcNow.Ticks}";
        var sig = ComputeMd5(payload + secret);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{payload}:{sig}"));
    }
}
