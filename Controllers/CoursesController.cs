using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public IActionResult Create(CreateCourseDto dto)
    {
        var teacherId = int.Parse(User.FindFirst("nameid").Value);

        var teacher = _context.Users.Find(teacherId);
        var subject = _context.Subjects.Find(dto.SubjectId);
        var cls = _context.Classes.Find(dto.ClassId);

        if (teacher == null || subject == null || cls == null)
            return BadRequest("Invalid data");

        // Генерация названия
        var year = DateTime.Now.Year;
        var courseName = $"{year} {cls.Name} {teacher.Nickname} {subject.Name}";

        var course = new Course
        {
            SubjectId = dto.SubjectId,
            TeacherId = teacherId,
            ClassId = dto.ClassId,
            IsLK = dto.IsLK
        };

        _context.Courses.Add(course);
        _context.SaveChanges();

        return Ok(new { course, name = courseName });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.Courses.ToList());
    }

    // Get courses for student
    [HttpGet("my")]
    public IActionResult GetMyCourses()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized("No userId");

        var userId = int.Parse(userIdClaim.Value);

        var user = _context.Users.Find(userId);

        if (user == null)
            return NotFound("User not found");

        if (user.ClassId == null)
            return BadRequest("User has no class");

        var courses = _context.Courses
            .Include(c => c.Subject)
            .Where(c => c.ClassId == user.ClassId)
            .ToList();

        return Ok(courses);
    }
}