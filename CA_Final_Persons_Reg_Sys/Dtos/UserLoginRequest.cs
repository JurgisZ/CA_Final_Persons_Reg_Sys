namespace CA_Final_Persons_Reg_Sys.Dtos
{
    public class UserLoginRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
