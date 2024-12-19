namespace CA_Final_Persons_Reg_Sys.Dtos
{
    public class UserLoginResult
    {
        public required long Id { get; set; }
        public required string UserName { get; set; }
        public string Role { get; set; } = "user";
    }
}
