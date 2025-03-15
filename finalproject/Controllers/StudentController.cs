using finalproject.Models;
using Microsoft.AspNetCore.Mvc;
using finalproject.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace finalproject.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult StudentLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> StudentLogin(StudentLoginViewModel model)
        {
        if (ModelState.IsValid)
        {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordHash == model.Password);

        if (user != null)
        {
            HttpContext.Session.SetString("UserEmail", model.Email);
            return RedirectToAction("StudentCoursePage");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
        }
        }
            return View(model);
        }
        // course/indexe yönlendirme
        public async Task<IActionResult> StudentCoursePage()
        {
        if (!User.Identity.IsAuthenticated)
        {
        return RedirectToAction("StudentLogin", "Student");
        }
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Room)
                .ToListAsync();
            
            return View(courses);
        }

        public async Task UpdateCoursePhoto(){
            var courses = await _context.Courses.ToListAsync();

            foreach(var course in courses){

                if(course.PhotoUrl==null){
                    course.PhotoUrl = "img/default.jpg";
                }
                if(course.CourseID == "BAS1"){
                    course.PhotoUrl = "/img/basketbol3.jpg";
                } else if (course.CourseID == "FIT1"){
                    course.PhotoUrl = "/img/fitness4.jpeg";
                }

                await _context.SaveChangesAsync();
            }
        }
         public async Task<IActionResult> CoursePage()
        {
            await UpdateCoursePhoto();
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

    [HttpPost]
        public async Task<IActionResult> ApplyForCourse(CourseApplyViewModel model)
    {
        if (!User.Identity.IsAuthenticated)
        {
        return RedirectToAction("StudentLogin", "Student");
        }
        if (ModelState.IsValid)
        {
        var email = HttpContext.Session.GetString("UserEmail");
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.CourseID == model.CourseID);
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentEmail == email);
        if (course != null && student != null)
        {
            var enrollment = new Enrollment
            {
                CourseID = course.CourseID,
                StudentID = student.StudentID,
                EnrollmentDate = DateTime.Now
            };
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Success");
        }
        else
        {
            ModelState.AddModelError("", "Kurs veya öğrenci bulunamadı.");
        }
        }
            return RedirectToAction("StudentCoursePage");
    }

        public IActionResult Success()
    {
        return View();
    } 

    }
}
