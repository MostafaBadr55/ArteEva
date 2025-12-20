using ArtEva.Helpers;
using ArtEva.Services.Interfaces;

namespace ArtEva.Services.Implementations
{
    public class FileService : IFileService
    {

        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public FileService(IConfiguration config ,IWebHostEnvironment webHostEnvironment )
        {
            _config = config;
            _env = webHostEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string type)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Invalid file");

              var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(ext))
                throw new Exception("File type not allowed");

            // Get folder name based on type
            var folderName = PathHelper.GetFolderByType(type);

          
            // /wwwroot/uploads/products

            var uploadRoot = _config["UploadSettings:Root"];
            var finalFolder = Path.Combine(uploadRoot, folderName);

            Directory.CreateDirectory(finalFolder);

            var newFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(finalFolder, newFileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var baseUrl = _config["UploadSettings:BaseUrl"];
            /// Example:
            /// http://localhost:5000/uploads/Shops/uuid.jpg
            /// {baseUrl}/uploads/{folderName}/
            return newFileName;
        }
    }
}
