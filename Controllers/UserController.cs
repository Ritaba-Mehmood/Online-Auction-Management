using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SemesterProj.Data;
using SemesterProj.Models;
using System.Security.Claims;

namespace SemesterProj.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult MyBid()
        {
            return View();
        }
        public IActionResult Profile()
        {
            // Handle possible null reference
            if (User.Identity?.Name == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var username = User.Identity.Name;
            var user = _context.Auth.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

     
        [HttpGet("User/Sell")]
        public IActionResult Sell()
        {
            // Load categories into ViewBag
            var categories = _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(new Item());
        }

        [HttpPost("User/Sell")]
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

                item.Image = $"/uploads/{fileName}";
            }

            if (ModelState.IsValid)
            {
                var username = User.Identity?.Name;
                var user = _context.Auth.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    item.SellerId = user.Id;
                }

                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Item listed successfully!";
                return RedirectToAction("Profile", "User");
            }

            var categories = _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(item);
        }
    }
}