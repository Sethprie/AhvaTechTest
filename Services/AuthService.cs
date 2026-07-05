using AhvaTechTest.Repositories;
using BCrypt.Net;

namespace AhvaTechTest.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private const int MaxFailedAttempts = 5;
        private const int LockMinutes = 15;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResult> LoginAsync(string documentType, string documentNumber, string password)
        {
            var user = await _userRepository.GetByDocumentAsync(documentType, documentNumber);

            if (user == null)
            {
                return new LoginResult { Status = LoginStatus.InvalidCredentials };
            }

            if (user.IsLocked && user.LockedAt.HasValue)
            {
                var unlockTime = user.LockedAt.Value.AddMinutes(LockMinutes);

                if (DateTime.Now < unlockTime)
                {
                    var remaining = (int)Math.Ceiling((unlockTime - DateTime.Now).TotalMinutes);
                    return new LoginResult
                    {
                        Status = LoginStatus.AccountLocked,
                        RemainingLockMinutes = remaining
                    };
                }

                user.IsLocked = false;
                user.LockedAt = null;
                user.FailedAttemptsCount = 0;
                await _userRepository.UpdateAsync(user);
            }

            bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (passwordMatches)
            {
                if (user.FailedAttemptsCount > 0)
                {
                    user.FailedAttemptsCount = 0;
                    await _userRepository.UpdateAsync(user);
                }

                return new LoginResult { Status = LoginStatus.Success, User = user };
            }

            user.FailedAttemptsCount++;

            if (user.FailedAttemptsCount >= MaxFailedAttempts)
            {
                user.IsLocked = true;
                user.LockedAt = DateTime.Now;
                await _userRepository.UpdateAsync(user);


                return new LoginResult
                {
                    Status = LoginStatus.AccountJustLocked,
                    RemainingLockMinutes = LockMinutes
                };
            }

            await _userRepository.UpdateAsync(user);
            return new LoginResult { Status = LoginStatus.InvalidCredentials };
        }
    }
}