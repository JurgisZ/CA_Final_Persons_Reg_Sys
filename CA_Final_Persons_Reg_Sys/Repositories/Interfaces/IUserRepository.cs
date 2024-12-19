using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;

namespace CA_Final_Persons_Reg_Sys.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<long> CreateAsync(User user);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<User>> GetAsync();
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByUserName(string userName);
        Task<string?> GetUserPictureUrl(long id);

        //Task<bool> UpdateAsync(long id, UserRequest User);
        //Task<bool> UpdatePersonalDataPropertyAsync(long id, string propertyName, object value);
        Task<bool> UpdateUserApartmentNumber(long id, string apartmentNumber);
        Task<bool> UpdateUserCityName(long id, string city);
        Task<bool> UpdateUserEmail(long id, string email);
        Task<bool> UpdateUserHouseNumber(long id, string houseNumber);
        Task<bool> UpdateUserLastName(long id, string lastName);
        Task<bool> UpdateUserName(long id, string name);
        Task<bool> UpdateUserPassword(long id, byte[] passwordHash, byte[] passwordSalt);
        Task<bool> UpdateUserPersonalCode(long id, string personalCode);
        Task<bool> UpdateUserPhoneNumber(long id, string phoneNumber);
        Task<bool> UpdateUserPicture(long id, string pictureUrl);
        Task<bool> UpdateUserStreetName(long id, string street);
    }
}