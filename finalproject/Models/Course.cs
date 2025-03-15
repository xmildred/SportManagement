namespace finalproject.Models
{
    public class Course
    {
        public string CourseID { get; set; }
        public string CourseName { get; set; }
        public int Capacity { get; set; }
        public string InstructorID { get; set; }
        public Instructor Instructor { get; set; }
        public string RoomID { get; set; }
        public Room Room { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
