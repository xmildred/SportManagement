using finalproject.Models;
using Microsoft.AspNetCore.Mvc;
using finalproject.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace finalproject.Controllers
{
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
}