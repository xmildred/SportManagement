using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using finalproject.Data;
using finalproject.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace finalproject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Register(RegisterViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = new User
        {
            UserID = Guid.NewGuid().ToString(),
            UserName = model.StudentEmail,
            PasswordHash = model.Password, 
            Email = model.StudentEmail, 
            Role = "Student"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var student = new Student
        {
            StudentID = user.UserID, 
            StudentName = model.StudentName,
            StudentSurname = model.StudentSurname,
            StudentEmail = model.StudentEmail,
            StudentPhoneNumber = model.StudentPhoneNumber,
            UserID = user.UserID
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();    
        return RedirectToAction("Index", "Home"); 
    }

    return View(model);
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
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user != null)
    {
        if (user.PasswordHash == model.Password)
        {
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("Index", "Home");
        }
    }
        ModelState.AddModelError("", "Geçersiz email veya şifre.");
    }

    return View(model);
    }
    }
}
