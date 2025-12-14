namespace ArtEva.Services
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file, string type);
    }
}
