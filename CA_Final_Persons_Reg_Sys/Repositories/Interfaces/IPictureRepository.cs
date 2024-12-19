using CA_Final_Persons_Reg_Sys.Dtos;

namespace CA_Final_Persons_Reg_Sys.Repositories.Interfaces
{
    public interface IPictureRepository
    {
        Task<string?> CreateAsync(FileUploadRequest request);
        void Delete(string filePath);
        byte[]? GetByFileName(string fileName);
        string? GetContentType(string fileName);
        Task<string?> Update(FileUploadRequest request, string oldFilePath);
    }
}