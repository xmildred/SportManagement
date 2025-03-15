namespace finalproject.Models
{
    public class Instructor
    {
        public string InstructorID { get; set; }
        public string InstructorName { get; set; }
        public string InstructorSurname { get; set; }
        public string InstructorEmail { get; set; }
        public int InstructorPhoneNumber { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
