using AhvaTechTest.Data;
using AhvaTechTest.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AhvaTechTest.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users.FindAsync(userId.Value);

            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Auth");
            }

            var viewModel = new ProfileViewModel
            {
                FullName = user.FullName ?? "",
                Position = user.Position ?? "",
                Entity = user.Entity ?? "",
                Status = user.Status ?? ""
            };

            return View(viewModel);
        }
    }
}