using Azure.Core;
using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;

namespace CA_Final_Persons_Reg_Sys.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        public async Task<string?> Create(FileUploadRequest request)
        {
            var uploadPath = "uploads/";    //move to json

            var fileExtension = Path.GetExtension(request.File.FileName);
            if (string.IsNullOrEmpty(fileExtension))
            {
                return null;
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}"; // Generate unique file name
            var filePath = Path.Combine(uploadPath, fileName);

            if (request.File == null || request.File.Length == 0)
            {
                return null;
            }

            try
            {
                Directory.CreateDirectory(uploadPath); // Ensure the directory exists
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await request.File.CopyToAsync(fileStream); // Directly copy the stream
            }
            catch (Exception ex)
            {
                return null;
            }

            return fileName;
        }


        public async Task<string?> Update(FileUploadRequest request, string oldFilePath)
        {
            var uploadPath = "uploads/";    //move to json

            //I try.. catch
            if (request.File == null || request.File.Length == 0)
            {
                return null;
            }


            var fileExtension = Path.GetExtension(request.File.FileName);
            if (string.IsNullOrEmpty(fileExtension))
            {
                return null;
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}"; // New file name
            var filePath = Path.Combine(uploadPath, fileName);

            try
            {
                Directory.CreateDirectory(uploadPath); // Ensure the directory exists
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await request.File.CopyToAsync(fileStream); // Directly copy the stream
                File.Delete(Path.Combine(uploadPath, oldFilePath));
            }
            catch (Exception ex)
            {
                return null;
            }

            return fileName;

        }

        public void Delete(string filePath)
        {
            var uploadPath = "uploads/";        ///JSON!!!!!
            var fullPath = Path.Combine(uploadPath, filePath);
            try
            {
                File.Delete(uploadPath);
            }

            catch (Exception ex)
            {
                
            }
            return;
        }
    }
}
