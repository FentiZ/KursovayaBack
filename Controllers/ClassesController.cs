using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]

public class ClassesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClassesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(CreateClassDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Name required");

        var name = dto.Name.ToUpper();

        var isNormal = System.Text.RegularExpressions.Regex.IsMatch(name, @"^\d{1,2}[A-Z]$"); // 10A
        var isSpecial = name == "EF" || name == "Q1" || name == "Q2";

        if (!isNormal && !isSpecial)
            return BadRequest("Invalid class format");

        var cls = new Class
        {
            Name = name
        };

        _context.Classes.Add(cls);
        _context.SaveChanges();

        return Ok(cls);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public IActionResult GetAll()
    {
        return Ok(_context.Classes.ToList());
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var cls = _context.Classes.Find(id);

        if (cls == null)
            return NotFound();

        _context.Classes.Remove(cls);
        _context.SaveChanges();

        return Ok();
    }
}

