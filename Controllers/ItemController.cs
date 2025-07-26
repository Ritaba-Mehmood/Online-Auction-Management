using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SemesterProj.Data;
using SemesterProj.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace SemesterProj.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context )
        {
            _context = context;
          
        }
        [HttpGet]
        
        public IActionResult Sell()
        {
            LoadCategories();
            return View();
        }

        public IActionResult AllItems()
        {
            ViewData["HideFooter"] = true;
            var items = _context.Items.Include(i => i.Category).Include(i => i.Seller).ToList(); // Adjust based on your models
            return View("~/Views/Admin/AllItems.cshtml", items);
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

            return View("~/Views/Home/Details.cshtml", item);
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _context.Categories
                    .AsNoTracking()
                    .OrderBy(c => c.Name)
                    .ToList();

                Console.WriteLine($"Fetched Categories Count: {categories.Count}");
                foreach (var category in categories)
                {
                    Console.WriteLine($"Category: {category.Id} - {category.Name}");
                }

                ViewBag.Categories = new SelectList(categories, "Id", "Name");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}");
                ViewBag.Categories = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Sell(Item item, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsDir)) Directory.CreateDirectory(uploadsDir);

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Set the database path to the uploaded file path
                item.Image = $"/uploads/{fileName}";
            }

            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Item listed successfully!";
                return RedirectToAction("Index", "ManageItems");
            }

            LoadCategories(); // Reload categories for the form
            return View(item);
        }

        private List<Categories> GetCategoriesSafe()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch
            {
                // Return empty list if categories can't be loaded
                return new List<Categories>();
            }
        }

        // ADMIN APPROVAL PAGE: GET /Item/Manage
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            var pendingItems = _context.Items
                .Include(i => i.Category)
                .Include(i => i.Seller)
                .Where(i => i.BidStatus == "Pending")
                .ToList();

            return View(pendingItems);
        }

        // APPROVE ACTION: POST /Item/Approve/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                item.BidStatus = "Active";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Manage");
        }

        // REJECT ACTION: POST /Item/Reject/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Manage");
        }
    }
}