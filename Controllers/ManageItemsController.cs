using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SemesterProj.Data;
using SemesterProj.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ManageItems
        public async Task<IActionResult> Index()
        {// In your AdminController actions:
            ViewData["HideFooter"] = true;
            var pendingItems = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Seller)
                .Where(i => i.BidStatus == "Pending")
                .ToListAsync();

            return View("~/Views/Admin/ManageItems.cshtml", pendingItems);
        }
        [HttpGet]
        public IActionResult ManageItems()
        {
            var items = _context.Items
                .Include(i => i.Category)
                .Include(i => i.Seller)
                .Where(i => i.BidStatus == "Pending")
                .ToList();

            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "imageData.json");
            var imageMetadataList = new List<dynamic>();

            if (System.IO.File.Exists(jsonFilePath))
            {
                var json = System.IO.File.ReadAllText(jsonFilePath);
                imageMetadataList = JsonConvert.DeserializeObject<List<dynamic>>(json) ?? new List<dynamic>();
            }

            foreach (var item in items)
            {
                var imageMetadata = imageMetadataList.FirstOrDefault(im => im.ItemId == item.ItemId);
                if (imageMetadata != null)
                {
                    item.Image = imageMetadata.ImagePath;
                }
            }

            return View("ManageItems", items); // Make sure to create a "ManageItems" view
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Seller)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
            {
                return NotFound(); // Returns 404 if item is not found
            }

            return View(item);
        }
        // POST: ManageItems/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.BidStatus = "Active";
            _context.Update(item);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Item approved successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: ManageItems/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Item rejected successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}