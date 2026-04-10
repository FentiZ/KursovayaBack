using Microsoft.EntityFrameworkCore;
using System.Linq;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // Tables
    public DbSet<User> Users { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonComment> LessonComments { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Attendance> Attendance { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMember> ChatMembers { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<SchoolAccount> SchoolAccounts { get; set; }
    public DbSet<Excuse> Excuses { get; set; }
    public DbSet<ScheduleEntry> ScheduleEntries { get; set; }
    public DbSet<CalendarEvent> CalendarEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ❗ Отключаем каскадное удаление
        foreach (var foreignKey in modelBuilder.Model
                     .GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
        }

        //Class -> Students (One-to-Many)
        modelBuilder.Entity<Class>()
            .HasMany(c => c.Students)
            .WithOne(u => u.Class)
            .HasForeignKey(u => u.ClassId);

        //Course связи
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany()
            .HasForeignKey(c => c.TeacherId);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Subject)
            .WithMany()
            .HasForeignKey(c => c.SubjectId);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Class)
            .WithMany()
            .HasForeignKey(c => c.ClassId);
    }
}