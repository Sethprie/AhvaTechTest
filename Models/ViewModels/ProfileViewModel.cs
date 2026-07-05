namespace AhvaTechTest.Models.ViewModels
{
    public class ProfileViewModel
    {
        // Header (ya existe, viene de sesión)
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        // Columna 1
        public string FirstNames { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string? SecondaryEmail { get; set; }
        public string ContractType { get; set; } = string.Empty;

        // Columna 2
        public string LastNameFirst { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty; // editable
        public DateTime HireDate { get; set; }

        // Columna 3
        public string LastNameSecond { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string PrimaryEmail { get; set; } = string.Empty; // editable
        public string? SecondaryPhone { get; set; }
        public string? SecondaryPhoneType { get; set; }
    }
}