using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;

namespace CA_Final_Persons_Reg_Sys.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<long> CreateAsync(User user);
        void DeleteAsync(long id);
        Task<IEnumerable<User>> GetAsync();
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByUserNameAsync(string userName);
        Task<string?> GetUserPictureUrl(long id);

        //Task<bool> UpdateAsync(long id, UserRequest User);
        //Task<bool> UpdatePersonalDataPropertyAsync(long id, string propertyName, object value);
        Task<bool> UpdateUserApartmentNumberAsync(long id, string apartmentNumber);
        Task<bool> UpdateUserCityNameAsync(long id, string city);
        Task<bool> UpdateUserEmailAsync(long id, string email);
        Task<bool> UpdateUserHouseNumberAsync(long id, string houseNumber);
        Task<bool> UpdateUserLastNameAsync(long id, string lastName);
        Task<bool> UpdateUserNameAsync(long id, string name);
        Task<bool> UpdateUserPassword(long id, byte[] passwordHash, byte[] passwordSalt);
        Task<bool> UpdateUserPersonalCodeAsync(long id, string personalCode);
        Task<bool> UpdateUserPhoneNumberAsync(long id, string phoneNumber);
        Task<bool> UpdateUserPicture(long id, string pictureUrl);
        Task<bool> UpdateUserStreetNameAsync(long id, string street);
    }
}