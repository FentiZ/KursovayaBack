public class ChatMember
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public ChatRoom Room { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}