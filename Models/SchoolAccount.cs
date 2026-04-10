public class SchoolAccount
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public User Student { get; set; }

    public string Title { get; set; }

    public string Login { get; set; }
    public string Password { get; set; }
}