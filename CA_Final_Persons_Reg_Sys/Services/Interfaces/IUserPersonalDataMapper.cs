using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;

namespace CA_Final_Persons_Reg_Sys.Services.Interfaces
{
    public interface IUserPersonalDataMapper
    {
        UserPersonalDataRequest MapRequest(UserPersonalData model);
        UserPersonalDataResult MapResult(UserPersonalData model);
        void Project(UserPersonalData model, UserPersonalDataRequest dto);
    }
}