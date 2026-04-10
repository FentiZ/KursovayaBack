public class Course
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public int TeacherId { get; set; }
    public User Teacher { get; set; }

    public int ClassId { get; set; }
    public Class Class { get; set; }

    public bool IsLK { get; set; }
}