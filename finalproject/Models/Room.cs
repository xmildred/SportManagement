namespace finalproject.Models
{
    public class Room
    {
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public int RoomCapacity { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
