using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CA_Final_Persons_Reg_Sys.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IUserMapper _mapper;

        public UserService(IUserRepository repo, IUserMapper mapper, IJwtService jwtService)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<long?> CreateAsync(UserCreateRequest request)
        {
            if (request == null || request.userPersonalDataRequest == null)
                return null;

            var existingUser = await _repo.GetByUserNameAsync(request.UserName);
            if (existingUser != null)
                return null;

            CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
            var entity = _mapper.MapToEntity(request, passwordHash, passwordSalt);

            return await _repo.CreateAsync(entity);
        }

        public async Task<IEnumerable<UserResult>> GetUsers()
        {
            var users = await _repo.GetAsync();

            return _mapper.MapResult(users);
        }

        public async Task<User?> GetById(long id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<UserResult?> GetByUserName(string userName)
        {
            var entity = await _repo.GetByUserNameAsync(userName);
            if (entity == null)
                return null;

            return _mapper.MapResult(entity);
        }

        public async Task<string?> GetUserPictureUrlByUserId(long id)
        {
            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)
                return null;

            return existingUser.UserPersonalData.ProfilePicture;
        }

        public async Task<bool> UpdateUserPicture(long id, string fileName)
        {
            return await _repo.UpdateUserPicture(id, fileName);
        }

        public async Task<bool> UpdateUserPassword(long id, string password)
        {
            if (string.IsNullOrEmpty(password) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)
                return false;

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            return await _repo.UpdateUserPassword(id, passwordHash, passwordSalt);
        }

        public async Task<bool> UpdateUserName(long id, string name)
        {
            if (string.IsNullOrEmpty(name) || id < 1)
                return false;

            var existingUserSameName = await _repo.GetByUserNameAsync(name);
            if (existingUserSameName != null)   //su tokiu vardu jau yra
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)           //neegzistuoja
                return false;


            return await _repo.UpdateUserNameAsync(id, name);
        }

        public async Task<bool> UpdateUserLastName(long id, string lastName)
        {
            if (string.IsNullOrEmpty(lastName) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)
                return false;

            return await _repo.UpdateUserLastNameAsync(id, lastName);
        }

        public async Task<bool> UpdateUserPersonalCode(long id, string personalCode)
        {
            if (string.IsNullOrEmpty(personalCode) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)
                return false;
            return await _repo.UpdateUserPersonalCodeAsync(id, personalCode);
        }

        public async Task<bool> UpdateUserPhoneNumber(long id, string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)
                return false;
            return await _repo.UpdateUserPhoneNumberAsync(id, phoneNumber);
        }

        public async Task<bool> UpdateUserEmail(long id, string email)
        {
            if (string.IsNullOrEmpty(email) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null || email == null)
                return false;
            return await _repo.UpdateUserEmailAsync(id, email);
        }

        public async Task<bool> UpdateUserCityName(long id, string cityName)
        {
            if (string.IsNullOrEmpty(cityName) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null || string.IsNullOrEmpty(cityName))
                return false;
            return await _repo.UpdateUserCityNameAsync(id, cityName);
        }

        public async Task<bool> UpdateUserStreetName(long id, string streetName)
        {
            if (string.IsNullOrEmpty(streetName) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)
                return false;
            return await _repo.UpdateUserStreetNameAsync(id, streetName);
        }

        public async void DeleteUser(long id)
        {
            if (id < 1) return;
            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)
                return;

            _repo.DeleteAsync(id);
        }

        public async Task<bool> UpdateUserHouseNumber(long id, string houseNumber)
        {
            if (string.IsNullOrEmpty(houseNumber) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null)
                return false;
            return await _repo.UpdateUserHouseNumberAsync(id, houseNumber);
        }

        public async Task<bool> UpdateUserApartmentNumber(long id, string apartmentNumber)
        {
            if (string.IsNullOrEmpty(apartmentNumber) || id < 1)
                return false;

            var existingUser = await _repo.GetByIdAsync(id);
            if (existingUser == null || string.IsNullOrEmpty(apartmentNumber))
                return false;
            return await _repo.UpdateUserApartmentNumberAsync(id, apartmentNumber);
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

        public async Task<UserLoginResult?> Login(UserLoginRequest request)
        {
            var user = await _repo.GetByUserNameAsync(request.UserName);
            if (user == null)
                return null;

            if(VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return _mapper.MapLoginResult(user);
            
            return null;            
        }




    }
}
