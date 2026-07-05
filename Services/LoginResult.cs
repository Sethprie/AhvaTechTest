using AhvaTechTest.Models;

namespace AhvaTechTest.Services
{
    public enum LoginStatus
    {
        Success,
        InvalidCredentials,
        AccountLocked,
        AccountJustLocked
    }

    public class LoginResult
    {
        public LoginStatus Status { get; set; }
        public User? User { get; set; }
        public int RemainingLockMinutes { get; set; }
    }
}