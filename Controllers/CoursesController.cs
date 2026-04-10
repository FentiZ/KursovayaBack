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

    //CREATE COURSE
    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public IActionResult Create(CreateCourseDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized("No userId");

        var teacherId = int.Parse(userIdClaim.Value);

        var teacher = _context.Users.Find(teacherId);
        var subject = _context.Subjects.Find(dto.SubjectId);
        var cls = _context.Classes.Find(dto.ClassId);

        if (teacher == null || subject == null || cls == null)
            return BadRequest("Invalid data");

        //Генерация названия курса
        var year = DateTime.Now.Year;
        var courseName = $"{year} {cls.Name} {teacher.Nickname} {subject.Name}";

        var course = new Course
        {
            Name = courseName, //сохраняем
            SubjectId = dto.SubjectId,
            TeacherId = teacherId,
            ClassId = dto.ClassId,
            IsLK = dto.IsLK
        };

        _context.Courses.Add(course);
        _context.SaveChanges();

        return Ok(course);
    }

    //ALL COURSES
    [HttpGet]
    public IActionResult GetAll()
    {
        var courses = _context.Courses
            .Include(c => c.Subject)
            .Include(c => c.Class)
            .Include(c => c.Teacher)
            .ToList();

        return Ok(courses);
    }

    //MY COURSES (student)
    [HttpGet("my")]
    public IActionResult GetMyCourses()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var user = _context.Users.Find(userId);

        if (user == null)
            return NotFound("User not found");

        // TEACHER
        if (user.Role == "Teacher")
        {
            var courses = _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Class)
                .Include(c => c.Teacher)
                .Where(c => c.TeacherId == userId)
                .ToList();

            return Ok(courses);
        }

        // STUDENT
        if (user.ClassId == null)
            return BadRequest(new { message = "User has no class" });

        var studentCourses = _context.Courses
            .Include(c => c.Subject)
            .Include(c => c.Class)
            .Where(c => c.ClassId == user.ClassId)
            .ToList();

        return Ok(studentCourses);
    }
}