namespace AhvaTechTest.Services
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(string documentType, string documentNumber, string password);
    }
}