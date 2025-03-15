using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using finalproject.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(3,3,0)))); // Güncel MySQL versiyonunu kullanın

// Configure session services
builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie accessible only via HTTP
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Configure authentication with Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Admin/Login"; // Admin giriş sayfasının yolu
    })
    .AddCookie("StudentScheme", options =>
    {
        options.LoginPath = "/Student/StudentLogin"; // Öğrenci giriş sayfasının yolu
    })
    .AddCookie("InstructorScheme", options =>
    {
        options.LoginPath = "/Instructor/InstructorLogin"; // Eğitmen giriş sayfasının yolu
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doğrulamasını ekleyin
app.UseAuthorization();
app.UseSession(); // Oturumu ekleyin

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "course",
    pattern: "courseList",
    defaults: new { controller = "Course", action = "CoursePage" });

// Güncellenmiş admin yönlendirmesi
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Login}/{id?}",
    defaults: new { controller = "Admin" });

app.Run();

