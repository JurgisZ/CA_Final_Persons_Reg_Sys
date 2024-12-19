using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;

namespace CA_Final_Persons_Reg_Sys.Services.Interfaces
{
    public interface IUserService
    {
        Task<long?> CreateAsync(UserCreateRequest request);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        void DeleteUser(long id);
        Task<User?> GetById(long id);
        Task<UserResult?> GetByUserName(string userName);
        Task<string?> GetUserPictureUrlByUserId(long id);
        Task<IEnumerable<UserResult>> GetUsers();
        Task<UserLoginResult?> Login(UserLoginRequest request);
        Task<bool> UpdateUserApartmentNumber(long id, string apartmentNumber);
        Task<bool> UpdateUserCityName(long id, string cityName);
        Task<bool> UpdateUserEmail(long id, string email);
        Task<bool> UpdateUserHouseNumber(long id, string houseNumber);
        Task<bool> UpdateUserLastName(long id, string lastName);
        Task<bool> UpdateUserName(long id, string name);
        Task<bool> UpdateUserPassword(long id, string password);
        Task<bool> UpdateUserPersonalCode(long id, string personalCode);
        Task<bool> UpdateUserPhoneNumber(long id, string phoneNumber);
        Task<bool> UpdateUserPicture(long id, string fileName);
        Task<bool> UpdateUserStreetName(long id, string streetName);
    }
}