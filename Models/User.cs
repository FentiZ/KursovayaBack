public class User
{
    public int Id { get; set; }

    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string PreviousPasswordHash { get; set; }
    public string ParentPasswordHash { get; set; }

    public string Nickname { get; set; }
    public string Role { get; set; } // Admin, Teacher, Student

    public int Age { get; set; }

    public string AvatarUrl { get; set; }
    public string Status { get; set; }
    public DateTime LastOnline { get; set; }

    public string Theme { get; set; }

    public int? ClassId { get; set; }
    public Class Class { get; set; }
}