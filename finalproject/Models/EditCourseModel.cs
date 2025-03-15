namespace finalproject.Models
{
    public class EditCourseModel
    {
        public string CourseID { get; set; }
        public string CourseName { get; set; }
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RoomID { get; set; }

    }
}
