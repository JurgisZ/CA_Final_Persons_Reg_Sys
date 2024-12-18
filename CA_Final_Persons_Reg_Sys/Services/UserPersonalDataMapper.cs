using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;

namespace CA_Final_Persons_Reg_Sys.Services
{
    public class UserPersonalDataMapper : IUserPersonalDataMapper
    {
        public void Project(UserPersonalData model, UserPersonalDataRequest dto)
        {
            model.Name = dto.Name;
            model.LastName = dto.LastName;
            model.PersonalCode = dto.PersonalCode;
            model.PhoneNumber = dto.PhoneNumber;
            model.Email = dto.Email;
            model.ProfilePicture = dto.ProfilePicture;
            model.CityName = dto.CityName;
            model.StreetName = dto.StreetName;
            model.HouseNumber = dto.HouseNumber;
            model.ApartmentNumber = dto.ApartmentNumber;
        }

        public UserPersonalDataRequest MapRequest(UserPersonalData model)
        {
            return new UserPersonalDataRequest
            {
                Name = model.Name,
                LastName = model.LastName,
                PersonalCode = model.PersonalCode,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                ProfilePicture = model.ProfilePicture,
                CityName = model.CityName,
                StreetName = model.StreetName,
                HouseNumber = model.HouseNumber,
                ApartmentNumber = model.ApartmentNumber,
            };
        }
        public UserPersonalDataResult MapResult(UserPersonalData model)
        {
            return new UserPersonalDataResult
            {
                Id = model.Id,
                Name = model.Name,
                LastName = model.LastName,
                PersonalCode = model.PersonalCode,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                ProfilePicture = model.ProfilePicture,
                CityName = model.CityName,
                StreetName = model.StreetName,
                HouseNumber = model.HouseNumber,
                ApartmentNumber = model.ApartmentNumber
            };
        }
    }
}
