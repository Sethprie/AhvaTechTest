using AhvaTechTest.Data;
using AhvaTechTest.Models;
using Microsoft.EntityFrameworkCore;

namespace AhvaTechTest.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByDocumentAsync(string documentType, string documentNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.DocumentType == documentType &&
                    u.DocumentNumber == documentNumber);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}