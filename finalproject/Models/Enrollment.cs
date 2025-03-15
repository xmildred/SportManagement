namespace finalproject.Models
{
    public class Enrollment
    {
        //eşsiz bir enrollmentID oluşturdu
         public Guid EnrollmentID { get; set; } = Guid.NewGuid();
        public string CourseID { get; set; }
        public Course Course { get; set; }
        public string StudentID { get; set; }
        public Student Student { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int EnrollmentCount { get; set; }
    }
}
