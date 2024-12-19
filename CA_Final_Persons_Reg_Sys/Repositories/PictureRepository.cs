using Azure.Core;
using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;

namespace CA_Final_Persons_Reg_Sys.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        public async Task<string?> CreateAsync(FileUploadRequest request)
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
                Directory.CreateDirectory(uploadPath);
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await request.File.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                return null;
            }

            return fileName;
        }

        public bool Exists(string filePath)
        {
            const string uploadPath = "uploads/";   //appsettings.json
            try
            {
                var fullPath = Path.Combine(uploadPath, filePath);
                return System.IO.File.Exists(fullPath);
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public Byte[]? GetByFileName(string fileName)
        {
            var uploadPath = "uploads/";    //JSON!!!!
            try
            {
                var filePath = Path.Combine(uploadPath, fileName);
                if (!System.IO.File.Exists(filePath))
                    return null;

                var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
                
                return System.IO.File.ReadAllBytes(filePath);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public string? GetContentType(string fileName)
        {
            var uploadPath = "uploads/";    //appsettings.json

            var filePath = Path.Combine(uploadPath, fileName);
            if (!System.IO.File.Exists(filePath))
                return null;

            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            string contentType;
            switch (fileExtension)  //JSON!!!!!
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    return contentType;
                default:
                    return null;
            }
        }


        public async Task<string?> Update(FileUploadRequest request, string oldFilePath)
        {
            var uploadPath = "uploads/";    //move to json
            
            try
            { 
                var fileExtension = Path.GetExtension(request.File.FileName);
                if (string.IsNullOrEmpty(fileExtension))
                {
                    return null;
                }

                var fileName = $"{Guid.NewGuid()}{fileExtension}"; // New file name
                var filePath = Path.Combine(uploadPath, fileName);

                Directory.CreateDirectory(uploadPath); // Ensure the directory exists
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await request.File.CopyToAsync(fileStream); // Directly copy the stream
                File.Delete(Path.Combine(uploadPath, oldFilePath));
                return fileName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Delete(string filePath)
        {
            var uploadPath = "uploads/";        ///JSON!!!!!
            var fullPath = Path.Combine(uploadPath, filePath);
            try
            {
                File.Delete(uploadPath);
                return;
            }

            catch (Exception ex)
            {
                return;
            }
        }
    }
}
