using CA_Final_Persons_Reg_Sys.Dtos;

namespace CA_Final_Persons_Reg_Sys.Services.Interfaces
{
    public interface IPictureService
    {
        Task<string?> CreatePicture(FileUploadRequest request);
        void DeletePicture(string filePath);
        string? GetContentTypeByFileName(string fileName);
        byte[]? GetPictureByFileName(string fileName);
        Task<string?> UpdatePicture(FileUploadRequest request, string oldFilePath);
    }
}