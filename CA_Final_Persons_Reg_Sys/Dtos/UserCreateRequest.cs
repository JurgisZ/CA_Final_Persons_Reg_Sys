namespace CA_Final_Persons_Reg_Sys.Dtos
{
    public class UserCreateRequest
    {
        //include passwords
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string Role { get; set; } = "User";
        public UserPersonalDataRequest userPersonalDataRequest { get; set; }
    }
}
