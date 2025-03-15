using Microsoft.AspNetCore.Mvc;
using finalproject.Models;
using finalproject.Data;
using Microsoft.AspNetCore.Http;

namespace finalproject.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult InstructorLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InstructorLogin(InstructorLoginViewModel model)
        {
            var instructor = _context.Instructors
            .SingleOrDefault(i => i.InstructorEmail == model.Email && i.InstructorID == model.Password);

            if (instructor != null)
            {
                // Oturum açma işlemi başarılıysa
            HttpContext.Session.SetString("InstructorID", instructor.InstructorID);
            return RedirectToAction("InstructorStudents"); 
            }

    
        ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
        return View(model);
        }

        public IActionResult InstructorStudents()
    {
    
    var instructorId = HttpContext.Session.GetString("InstructorID");
    
    var courses = _context.Courses
        .Where(c => c.InstructorID == instructorId)
        .ToList();

    var studentCourses = _context.Students
        .Where(s => courses.Select(c => c.CourseID).Contains(s.RegisteredCourse))
        .ToList();

    var studentViewModels = studentCourses
        .Select(s => new StudentCourseViewModel
        {
            StudentID = s.StudentID,
            StudentName = s.StudentName,
            StudentSurname = s.StudentSurname,
            CourseID = s.RegisteredCourse,
            CourseName = courses.First(c => c.CourseID == s.RegisteredCourse).CourseName,
            StartDate = courses.First(c => c.CourseID == s.RegisteredCourse).StartDate.ToString("yyyy-MM-dd"),
            EndDate = courses.First(c => c.CourseID == s.RegisteredCourse).EndDate.ToString("yyyy-MM-dd")
        }).ToList();

    return View(studentViewModels);
    }

    }
}
