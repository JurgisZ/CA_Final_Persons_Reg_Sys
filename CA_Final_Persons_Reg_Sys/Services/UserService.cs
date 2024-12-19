using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;

namespace CA_Final_Persons_Reg_Sys.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly IUserMapper _mapper;
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UserResult> GetUsers()
        {
            var users = await _repo.GetAsync();


        }



        
    }
}
