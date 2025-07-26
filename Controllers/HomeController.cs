using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SemesterProj.Data;
using SemesterProj.Models;
using System.Diagnostics;

namespace SemesterProj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var activeItems = _context.Items
                .Include(i => i.Category)
                .Include(i => i.Seller)
                .Where(i => i.BidStatus == "Active" && i.BidEndDate > DateTime.Now)
                .ToList();

            return View(activeItems);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult HowItWorks()
        {
            return View();
        }


        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Seller)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
            {
                return NotFound(); // Returns 404 if the item is not found
            }

            return View(item); // This will render Views/Home/Details.cshtml
        }

    }
}
