using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AhvaTechTest.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(5)]
        public string DocumentType { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public int FailedAttemptsCount { get; set; } = 0;

        public bool IsLocked { get; set; } = false;

        public DateTime? LockedAt { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }

        [StringLength(100)]
        public string? Position { get; set; }

        [StringLength(100)]
        public string? Entity { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }
    }
}