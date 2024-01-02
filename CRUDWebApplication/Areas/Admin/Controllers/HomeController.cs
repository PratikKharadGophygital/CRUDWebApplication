using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUDWebApplication.Areas.Admin.Controllers
{
    
    [Area("Admin")] // Declare the area
    [Authorize(Roles = "Admin")] // Who is access this page
    public class HomeController : Controller
    {
        //[Route("admin/home/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
