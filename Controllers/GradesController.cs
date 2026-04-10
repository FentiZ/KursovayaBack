using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly AppDbContext _context;

    public GradesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public IActionResult AddGrade(Grade grade)
    {
        _context.Grades.Add(grade);
        _context.SaveChanges();
        return Ok(grade);
    }

    [HttpGet("student/{studentId}")]
    public IActionResult GetGrades(int studentId)
    {
        var grades = _context.Grades
            .Where(g => g.StudentId == studentId)
            .ToList();

        return Ok(grades);
    }
}