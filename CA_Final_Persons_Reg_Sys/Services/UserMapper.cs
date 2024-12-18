using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;

namespace CA_Final_Persons_Reg_Sys.Services
{
    public class UserMapper : IUserMapper
    {
        private readonly IUserPersonalDataMapper _userPersonalDataMapper;
        public UserMapper(IUserPersonalDataMapper userPersonalDataMapper)
        {
            _userPersonalDataMapper = userPersonalDataMapper;
        }

        public UserResult MapResult(User user)
        {
            return new UserResult
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role,
                userPersonalDataResult = _userPersonalDataMapper.MapResult(user.UserPersonalData)
            };
        }

        public IEnumerable<UserResult> MapResult(IEnumerable<User> users)
        {
            return users.Select(MapResult);
        }

        public UserRequest MapRequest(User user)
        {
            return new UserRequest
            {
                UserName = user.UserName,
                Role = user.Role,
                userPersonalDataRequest = _userPersonalDataMapper.MapRequest(user.UserPersonalData)
            };
        }

        public IEnumerable<UserRequest> MapRequest(IEnumerable<User> users)
        {
            return users.Select(MapRequest);
        }

        public User MapToEntity(UserCreateRequest request, byte[] passwordHash, byte[] passwordSalt)
        {
            return  new User
            {
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserPersonalData = new UserPersonalData
                {
                    Name = request.userPersonalDataRequest.Name,
                    LastName = request.userPersonalDataRequest.LastName,
                    PersonalCode = request.userPersonalDataRequest.PersonalCode,
                    PhoneNumber = request.userPersonalDataRequest.PhoneNumber,
                    Email = request.userPersonalDataRequest.Email,
                    ProfilePicture = request.userPersonalDataRequest.ProfilePicture,
                    CityName = request.userPersonalDataRequest.CityName,
                    StreetName = request.userPersonalDataRequest.StreetName,
                    HouseNumber = request.userPersonalDataRequest.HouseNumber,
                    ApartmentNumber = request.userPersonalDataRequest.ApartmentNumber
                }
            };
        }



    }
}
