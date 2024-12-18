using CA_Final_Persons_Reg_Sys.Model;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;
using System.Security.Cryptography;

namespace CA_Final_Persons_Reg_Sys.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public UserAuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task<bool> Login(string username, string password)
        {
            User? user = await _userRepository.GetByUserName(username);
            if (user == null)
                return false;

            return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
        }
    }
}
