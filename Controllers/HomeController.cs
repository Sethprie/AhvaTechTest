using Microsoft.AspNetCore.Mvc;
using AhvaTechTest.Data;
using AhvaTechTest.Models.ViewModels;

namespace AhvaTechTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Hardcodeado solo para esta prueba: usuario Id=1 del seeder
            var user = _context.Users.Find(1);

            var firstName = "Usuario";

            if (user?.FullName != null && user.FullName.Contains(','))
            {
                // FullName tiene formato "Apellido Apellido, Nombres"
                var namesPart = user.FullName.Split(',')[1].Trim();
                firstName = namesPart.Split(' ')[0];
            }

            var viewModel = new WelcomeViewModel
            {
                FirstName = firstName
            };

            return View(viewModel);
        }
    }
}