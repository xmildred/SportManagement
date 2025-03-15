using finalproject.Models;
using Microsoft.AspNetCore.Mvc;
using finalproject.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace finalproject.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // course/indexe yönlendirme
        public async Task<IActionResult> CourseDirection()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor) // İlişkili Instructor verilerini dahil eder
                .Include(c => c.Room)        // İlişkili Room verilerini dahil eder
                .ToListAsync();
            
            return View(courses);
        }

        public async Task UpdateCoursePhoto()
        {
            var courses = await _context.Courses.ToListAsync();

            foreach (var course in courses)
            {
                if (course.PhotoUrl == null)
                {
                    course.PhotoUrl = "img/default.jpg";
                }
                if (course.CourseID == "BAS1")
                {
                    course.PhotoUrl = "/img/basketbol3.jpg";
                }
                else if (course.CourseID == "FIT1")
                {
                    course.PhotoUrl = "/img/fitness4.jpeg";
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> CoursePage()
        {
            // Fotoğrafları güncelle
            await UpdateCoursePhoto();

            // Kursları getir ve ViewModel'e dönüştür
            var courseViewModels = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Room)
                .Select(c => new CourseViewModel
                {
                    CourseID = c.CourseID,
                    CourseName = c.CourseName,
                    Capacity = c.Capacity,
                    InstructorName = c.Instructor.InstructorName,
                    InstructorSurname = c.Instructor.InstructorSurname,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd"),
                    RoomID = c.Room.RoomID,
                    PhotoUrl = c.PhotoUrl
                }).ToListAsync();

            return View(courseViewModels);
        }
    }
}
