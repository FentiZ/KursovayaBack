using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]

public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateUser(User user)
    {
        user.PasswordHash = HashPassword(user.PasswordHash);
        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(user);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAll()
    {
        return Ok(_context.Users.ToList());
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userIdClaim = User.FindFirst("id");

        if (userIdClaim == null)
            return Unauthorized("Invalid token");

        var userId = int.Parse(userIdClaim.Value);

        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.Login,
            user.Nickname,
            user.Role,
            user.Theme
        });
    }
}