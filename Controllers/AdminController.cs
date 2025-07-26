using Microsoft.AspNetCore.Mvc;

namespace SemesterProj.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {// In your AdminController actions:
            ViewData["HideFooter"] = true;
            return View();
        }
        public IActionResult Bid()
        {
            ViewData["HideFooter"] = true;
            // This will render the Admin/Bid.cshtml view
            return View();
        }
    }
}
