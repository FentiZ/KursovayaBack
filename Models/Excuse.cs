public class Excuse
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public User Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public DateTime Date { get; set; }

    public string Reason { get; set; }
    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }
}