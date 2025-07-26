using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SemesterProj.Data;
using SemesterProj.Models;

namespace SemesterProj.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public IActionResult Index()
        {// In your AdminController actions:
            ViewData["HideFooter"] = true;
            var categories = _context.Categories.ToList();
            return View("~/Views/Admin/Categories.cshtml", categories);
        }


        //[HttpPost]
        //public IActionResult CreateCategory(Categories categories)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Categories.Add(categories);
        //        _context.SaveChanges();
        //        TempData["Success"] = "Category created successfully!";
        //    }
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult CreateCategory(Categories category)
        {
            if (ModelState.IsValid)
            {
                // Normalize name and check for duplicates
                var normalizedName = category.Name.Trim().ToLower();

                if (_context.Categories.Any(c => c.Name.ToLower() == normalizedName))
                {
                    TempData["Error"] = "Category already exists!";
                    return RedirectToAction("Index");
                }

                try
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    TempData["Success"] = "Category added successfully!";
                }
                catch (DbUpdateException ex)
                {
                    TempData["Error"] = $"Database error: {ex.InnerException?.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Invalid category name";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditCategory(Categories category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["Success"] = "Category updated successfully!";
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                TempData["Success"] = "Category deleted successfully!";
            }
            return RedirectToAction("Index");
        }

    }
}
