namespace finalproject.Models
{
    public class Student
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string StudentEmail { get; set; }
        public string StudentPhoneNumber { get; set; }
        public string RegisteredCourse { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
