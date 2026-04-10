using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("create-user")]
    public IActionResult CreateUser([FromBody] CreateUserDto dto)
    {
        if (dto == null)
            return BadRequest("DTO is null");

        if (string.IsNullOrEmpty(dto.Login) || string.IsNullOrEmpty(dto.Password))
            return BadRequest("Login/Password required");

        var user = new User
        {
            Login = dto.Login,
            PasswordHash = Hash(dto.Password),
            PreviousPasswordHash = "",
            ParentPasswordHash = "",
            Nickname = dto.Nickname ?? "",
            Role = dto.Role ?? "User",
            Age = dto.Age,
            Theme = "dark",
            AvatarUrl = "",
            Status = "Offline",
            LastOnline = DateTime.Now
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(user);
    }

    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        return Ok(_context.Users.ToList());
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        _context.SaveChanges();

        return Ok();
    }

    private string Hash(string password)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}