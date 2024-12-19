namespace CA_Final_Persons_Reg_Sys.Services.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(long id, string username, string role);
    }
}