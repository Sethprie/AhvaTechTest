using Microsoft.EntityFrameworkCore;
using AhvaTechTest.Models;

namespace AhvaTechTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.DocumentType, u.DocumentNumber })
                .IsUnique();

            // Seed de usuarios de prueba
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    DocumentType = "DNI",
                    DocumentNumber = "07079879",
                    Username = "jmendoza",
                    PasswordHash = "AQAAAAIAAYagAAAAEJ8f3z9y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8=", // placeholder
                    FailedAttemptsCount = 0,
                    IsLocked = false,
                    FullName = "Mendoza Quispe, July Camila",
                    Position = "Administrador de Recursos",
                    Entity = "011 Ministerio de Salud",
                    Status = "Activo"
                },
                new User
                {
                    Id = 2,
                    DocumentType = "DNI",
                    DocumentNumber = "12345678",
                    Username = "bloqueado",
                    PasswordHash = "AQAAAAIAAYagAAAAEJ8f3z9y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8=", // placeholder
                    FailedAttemptsCount = 5,
                    IsLocked = true,
                    LockedAt = new DateTime(2026, 7, 5, 12, 0, 0),
                    FullName = "Usuario De Prueba, Bloqueado",
                    Position = "Operador",
                    Entity = "011 Ministerio de Salud",
                    Status = "Activo"
                }
            );
        }
    }
}