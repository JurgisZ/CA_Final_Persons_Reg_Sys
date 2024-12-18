namespace CA_Final_Persons_Reg_Sys.Services.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(string username, string role);
    }
}