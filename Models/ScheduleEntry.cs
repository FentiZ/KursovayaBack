public class ScheduleEntry
{
    public int Id { get; set; }

    public int ClassId { get; set; }
    public Class Class { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public int DayOfWeek { get; set; }
    public int LessonNumber { get; set; }

    public string WeekType { get; set; } // A/B
}