using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;

namespace CA_Final_Persons_Reg_Sys.Services.Interfaces
{
    public interface IUserMapper
    {
        IEnumerable<UserRequest> MapRequest(IEnumerable<User> users);
        UserRequest MapRequest(User user);
        IEnumerable<UserResult> MapResult(IEnumerable<User> users);
        UserResult MapResult(User user);
        User MapToEntity(UserCreateRequest request, byte[] passwordHash, byte[] passwordSalt);
    }
}