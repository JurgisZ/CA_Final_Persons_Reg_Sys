namespace CA_Final_Persons_Reg_Sys.Services.Interfaces
{
    public interface IUserAuthenticationService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        Task<bool> Login(string username, string password);
    }
}