using AhvaTechTest.Models.ViewModels;
using AhvaTechTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace AhvaTechTest.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.LoginAsync(
                model.DocumentType,
                model.DocumentNumber,
                model.Password);

            switch (result.Status)
            {
                case LoginStatus.Success:
                    HttpContext.Session.SetInt32("UserId", result.User!.Id);
                    HttpContext.Session.SetString("FullName", result.User.FullName ?? "");
                    return RedirectToAction("Index", "Profile");

                case LoginStatus.InvalidCredentials:
                    model.ErrorMessage = "Usuario y/o contraseña incorrectos.";
                    ModelState.AddModelError("DocumentNumber", "");
                    ModelState.AddModelError("Password", "");
                    return View(model);

                case LoginStatus.AccountLocked:
                    model.ErrorMessage = $"Cuenta bloqueada. Intenta nuevamente en {result.RemainingLockMinutes} minuto(s).";
                    return View(model);

                case LoginStatus.AccountJustLocked:
                    return View("AccountLocked");

                default:
                    model.ErrorMessage = "Ocurrió un error inesperado.";
                    return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}