using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AhvaTechTest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FailedAttemptsCount = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    LockedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Entity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DocumentNumber", "DocumentType", "Entity", "FailedAttemptsCount", "FullName", "IsLocked", "LockedAt", "PasswordHash", "Position", "Status", "Username" },
                values: new object[,]
                {
                    { 1, "07079879", "DNI", "011 Ministerio de Salud", 0, "Mendoza Quispe, July Camila", false, null, "AQAAAAIAAYagAAAAEJ8f3z9y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8=", "Administrador de Recursos", "Activo", "jmendoza" },
                    { 2, "12345678", "DNI", "011 Ministerio de Salud", 5, "Usuario De Prueba, Bloqueado", true, new DateTime(2026, 7, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEJ8f3z9y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8y8=", "Operador", "Activo", "bloqueado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DocumentType_DocumentNumber",
                table: "Users",
                columns: new[] { "DocumentType", "DocumentNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
