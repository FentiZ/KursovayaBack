using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChatController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("room")]
    public IActionResult CreateRoom(ChatRoom room)
    {
        _context.ChatRooms.Add(room);
        _context.SaveChanges();
        return Ok(room);
    }

    [HttpPost("message")]
    public IActionResult SendMessage(Message msg)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

        msg.UserId = userId;
        msg.SentAt = DateTime.Now;

        _context.Messages.Add(msg);
        _context.SaveChanges();

        return Ok(msg);
    }

    [HttpGet("{roomId}")]
    public IActionResult GetMessages(int roomId)
    {
        var messages = _context.Messages
            .Where(m => m.RoomId == roomId)
            .OrderBy(m => m.SentAt)
            .ToList();

        return Ok(messages);
    }

    [HttpGet("rooms")]
    public IActionResult GetRooms()
    {
        return Ok(_context.ChatRooms.ToList());
    }
}