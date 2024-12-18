using CA_Final_Persons_Reg_Sys.Dtos;

namespace CA_Final_Persons_Reg_Sys.Repositories.Interfaces
{
    public interface IPictureRepository
    {
        Task<string?> Create(FileUploadRequest request);
    }
}