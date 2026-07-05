using System.ComponentModel.DataAnnotations;

namespace AhvaTechTest.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Seleccione el tipo de documento")]
        public string DocumentType { get; set; } = "DNI";

        [Required(ErrorMessage = "Ingrese su número de documento")]
        [Display(Name = "Usuario")]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese su contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
    }
}