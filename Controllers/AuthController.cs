using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SemesterProj.Data;
using SemesterProj.Models;
using System.Security.Claims;

namespace SemesterProj.Controllers
{
    public class AuthController : Controller
    {
        private const string AuthScheme = "CookieAuth";
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new Auth
            {
                Username = string.Empty,
                Email = string.Empty,
                Password = string.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(Auth model)
        {
            ModelState.Remove("Email");

            if (ModelState.IsValid)
            {
                var user = _context.Auth.FirstOrDefault(u => u.Username == model.Username);

                if (user != null && user.Password == model.Password)
                {
                    // Create claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                        new Claim("UserId", user.Id.ToString()) // Add UserId claim
                    };

                    // Add role claim for admin
                    if (user.Username == "admin")
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }

                    // Create identity
                    var claimsIdentity = new ClaimsIdentity(claims, AuthScheme);

                    // Sign in user
                    await HttpContext.SignInAsync(
                        AuthScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true // Remember me option
                        });

                    // Redirect based on role
                    if (user.Username == "admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Auth model)
        {
            if (ModelState.IsValid)
            {
                _context.Auth.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Registration successful!";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}