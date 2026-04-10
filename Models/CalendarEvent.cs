public class CalendarEvent
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public string Title { get; set; }

    public DateTime Date { get; set; }

    public string Type { get; set; }
}