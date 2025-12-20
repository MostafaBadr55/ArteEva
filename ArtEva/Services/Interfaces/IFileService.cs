namespace ArtEva.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file, string type);
    }
}
