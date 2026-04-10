public class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public User Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public int Value { get; set; }
    public string Type { get; set; }

    public DateTime Date { get; set; }
}