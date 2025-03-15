using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using finalproject.Data; // ApplicationDbContext burada tanımlı
using finalproject.Models; // Model sınıflarını burada tanımladık
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace finalproject.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Admin giriş sayfası
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == model.Username && u.PasswordHash == model.Password);

            if (user != null && user.Role == "Admin")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("AdminPage");
            }

            ModelState.AddModelError("", "Yanlış kullanıcı adı veya şifre.");
            return View(model);
        }

        [Authorize(Roles = "Admin")]

        public IActionResult AdminPage()
        {
            return View();
        }

        // Kursları listeleme
        public async Task<IActionResult> ManageCourses()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        // Eğitmenleri listeleme
        public async Task<IActionResult> ManageInstructors()
        {
            var instructors = await _context.Instructors.ToListAsync();
            return View(instructors);
        }

        // Kurs ekleme
        public IActionResult CreateCourse()
        {
             var instructors = _context.Instructors.ToList();
            var rooms = _context.Rooms.ToList();
            ViewData["Instructors"] = _context.Instructors.ToList();
            ViewData["Rooms"] = _context.Rooms.ToList();
            return View();
        }
        //Kurs yaratma
        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course)
        {
           if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageCourses)); 
            }
            ViewData["Instructors"] = _context.Instructors.ToList();
            ViewData["Rooms"] = _context.Rooms.ToList();
            return View(course);
        }

        // Kurs düzenleme sayfası
        public IActionResult EditCourse(string courseId)
    {
        if (string.IsNullOrEmpty(courseId))
        {
        return NotFound();
        }
        var course = _context.Courses.FirstOrDefault(c => c.CourseID == courseId);
        if (course == null)
        {
        return NotFound();
        }
        var viewModel = new EditCourseModel
        {
        CourseID = course.CourseID,
        CourseName = course.CourseName,
        Capacity = course.Capacity,
        StartDate = course.StartDate, 
        EndDate = course.EndDate 
        };

    return View(viewModel);
    }

    // Kurs düzenleme işlemi
    [HttpPost]
    public IActionResult EditCourse(EditCourseModel model)
    {
        if (!ModelState.IsValid)
        {
        return View(model);
        }
        var course = _context.Courses.FirstOrDefault(c => c.CourseID == model.CourseID);
        if (course == null)
        {
        return NotFound();
        }
        course.CourseName = model.CourseName;
        course.Capacity = model.Capacity;
        course.StartDate = model.StartDate;
        course.EndDate = model.EndDate; 
        _context.Update(course);
        _context.SaveChanges();

        return RedirectToAction("ManageCourses");
    }

        // Kurs silme işlemi
    public IActionResult DeleteCourse(string courseId)
    {
        if (string.IsNullOrEmpty(courseId))
        {
        return NotFound();
        }
        var course = _context.Courses.FirstOrDefault(c => c.CourseID == courseId);
        
        if (course == null)
        {
        return NotFound();
        }
        _context.Courses.Remove(course);
        _context.SaveChanges();
    return RedirectToAction("ManageCourses");
    }
        // Eğitmen ekleme
        public IActionResult CreateInstructor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateInstructor(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageInstructors");
            }
            return View(instructor);
        }

        // Eğitmen düzenleme
        public async Task<IActionResult> EditInstructor(string id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> EditInstructor(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Update(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageInstructors");
            }
            return View(instructor);
        }

        // Eğitmen silme
        public async Task<IActionResult> DeleteInstructor(string id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageInstructors");
        }
        //Kullanıcıları yönetme 
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _context.Users.ToListAsync();
            var students = await _context.Students.ToListAsync();
            var instructors = await _context.Instructors.ToListAsync();

            var model = new ManageUsersViewModel
            {
                Users = users,
                Students = students,
                Instructors = instructors
            };

            return View(model);
        }

        // Admin Toggle
        [HttpPost]
        public async Task<IActionResult> ToggleAdminRole(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = (user.Role == "Admin") ? "user" : "Admin";
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageUsers");
        }

        
    }
}
