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
        Task<bool> UpdateAsync(long id, UserRequest User);
        Task<bool> UpdatePersonalDataPropertyAsync(long id, string propertyName, object value);
    }
}