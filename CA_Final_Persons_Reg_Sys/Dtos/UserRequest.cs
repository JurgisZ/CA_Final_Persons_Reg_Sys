using CA_Final_Persons_Reg_Sys.Model;
using System.ComponentModel.DataAnnotations;

namespace CA_Final_Persons_Reg_Sys.Dtos
{
    public class UserRequest
    {
        public required string UserName { get; set; }
        public string Role { get; set; } = "User";
        public UserPersonalDataRequest userPersonalDataRequest { get; set; }
    }
}
