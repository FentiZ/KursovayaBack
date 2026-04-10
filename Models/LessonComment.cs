public class LessonComment
{
    public int Id { get; set; }

    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
}