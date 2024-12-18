using CA_Final_Persons_Reg_Sys.Data;
using CA_Final_Persons_Reg_Sys.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CA_Final_Persons_Reg_Sys.Utils
{
    public class SeedHelperService
    {
        private readonly UsersDbContext _context;

        public SeedHelperService(UsersDbContext context)
        {
            _context = context;
        }

        public byte[] GenerateSalt(int lenght = 16)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[lenght];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                //magic: Bytes, start offset, bytes, start offset, count
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                return sha512.ComputeHash(saltedPassword);
            }
        }

        public void SeedUsersLocal()
        {
            //Check for development environment

            //Check if Users table is empty
            if (_context.Users.Any())   //await...
                return;

            var users = new[]
            {
                new { UserName = "spurgis" , Password = "pass123", Role = "User" }
            };

            var idIterator = 1;
            foreach (var user in users)
            {
                var salt = GenerateSalt();
                var saltedPass = HashPasswordWithSalt(user.Password, salt);
                
                var entity = new User
                {
                    Id = idIterator++,
                    UserName = user.UserName,
                    PasswordSalt = salt,
                    PasswordHash = saltedPass,
                    Role = user.Role
                    //UserPersonalData = 
                };

                _context.Users.Add(entity);
                _context.SaveChanges();
            }
        }
    }
}
