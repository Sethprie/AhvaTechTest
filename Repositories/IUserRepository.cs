using AhvaTechTest.Models;

namespace AhvaTechTest.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByDocumentAsync(string documentType, string documentNumber);
        Task UpdateAsync(User user);
    }
}