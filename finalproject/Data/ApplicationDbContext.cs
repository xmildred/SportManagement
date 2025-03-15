using finalproject.Models;
using Microsoft.EntityFrameworkCore;

namespace finalproject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Course - Instructor ilişkisi
            modelBuilder.Entity<Course>()
                .ToTable("course")  // Düzeltme: Tablo ismini "course" olarak güncelledim.
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Course - Room ilişkisi
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Room)
                .WithMany(r => r.Courses)
                .HasForeignKey(c => c.RoomID)
                .OnDelete(DeleteBehavior.Restrict);

            // Enrollment - Course ilişkisi
            modelBuilder.Entity<Enrollment>()
                .ToTable("enrollment")
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Restrict);

            // Enrollment - Student ilişkisi
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            // Instructor tablosu konfigürasyonu
            modelBuilder.Entity<Instructor>()
                .ToTable("instructor");

            // Room tablosu konfigürasyonu
            modelBuilder.Entity<Room>()
                .ToTable("room")
                .HasKey(r => r.RoomID);

            // Student tablosu konfigürasyonu
            modelBuilder.Entity<Student>()
                .ToTable("student");

            // User tablosu konfigürasyonu
            modelBuilder.Entity<User>()
                .ToTable("users");

            // Kolon isimlerini belirtme
            modelBuilder.Entity<Course>()
                .Property(c => c.CourseID)
                .HasColumnName("courseID");

            modelBuilder.Entity<Instructor>()
                .Property(i => i.InstructorID)
                .HasColumnName("instructorID");

            modelBuilder.Entity<Room>()
                .Property(r => r.RoomID)
                .HasColumnName("roomID");

            modelBuilder.Entity<Student>()
                .Property(s => s.StudentID)
                .HasColumnName("studentID");

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.EnrollmentID)
                .HasColumnName("enrollmentID");

            modelBuilder.Entity<User>()
                .Property(u => u.UserID)
                .HasColumnName("UserID");
        }
    }
}
