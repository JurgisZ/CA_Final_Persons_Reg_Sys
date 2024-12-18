using CA_Final_Persons_Reg_Sys.Model;
using System.ComponentModel.DataAnnotations;

namespace CA_Final_Persons_Reg_Sys.Dtos
{
    public class UserResult
    {
        public required long Id { get; set; }
        public required string UserName { get; set; }
        public required string Role {  get; set; }   //hmm
        public UserPersonalDataResult userPersonalDataResult { get; set; }        
    }
}
