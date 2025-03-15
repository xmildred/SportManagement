namespace finalproject.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public ICollection<Instructor> Instructors { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
