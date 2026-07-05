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
                Status = user.Status ?? "",

                FirstNames = "July Camila",
                DocumentType = user.DocumentType,
                Nationality = "Peruana",
                SecondaryEmail = null,
                ContractType = "CAS",

                LastNameFirst = "Mendoza",
                DocumentNumber = user.DocumentNumber,
                Gender = "Femenino",
                MobilePhone = "+51 999 999 999",
                HireDate = new DateTime(2015, 3, 9),

                LastNameSecond = "Quispe",
                BirthDate = new DateTime(1944, 4, 15),
                PrimaryEmail = "test@minsa.gob.pe",
                SecondaryPhone = null,
                SecondaryPhoneType = null
            };

            return View(viewModel);
        }
    }
}