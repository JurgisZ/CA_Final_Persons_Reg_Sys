using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Repositories;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;

namespace CA_Final_Persons_Reg_Sys.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _repo;

        public PictureService(IPictureRepository repo)
        {
            _repo = repo;
        }

        public async Task<string?> CreatePicture(FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return null;
            }

            return await _repo.CreateAsync(request);
        }

        public Byte[]? GetPictureByFileName(string fileName)
        {
            var uploadPath = "uploads/";    //JSON!!!!
            if (fileName == null)
                return null;
            return _repo.GetByFileName(fileName);
            //return File(fileBytes, contentType);
        }

        public String? GetContentTypeByFileName(string fileName)
        {
            return _repo.GetContentType(fileName);
        }

        public async Task<string?> UpdatePicture(FileUploadRequest request, string oldFilePath)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return null;
            }
            return await _repo.Update(request, oldFilePath);
        }

        public void DeletePicture(string filePath)
        {
            if (filePath == null)
                return;
            _repo.Delete(filePath);
        }
    }
}
