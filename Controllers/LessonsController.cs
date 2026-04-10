using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LessonsController(AppDbContext context)
    {
        _context = context;
    }

    // Teacher creates lesson (tile)
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public IActionResult Create(Lesson lesson)
    {
        lesson.LessonDate = DateTime.Now;

        _context.Lessons.Add(lesson);
        _context.SaveChanges();

        return Ok(lesson);
    }

    // Get lessons by course
    [HttpGet("{courseId}")]
    public IActionResult GetByCourse(int courseId)
    {
        var lessons = _context.Lessons
            .Where(l => l.CourseId == courseId)
            .OrderByDescending(l => l.LessonDate)
            .ToList();

        return Ok(lessons);
    }
}