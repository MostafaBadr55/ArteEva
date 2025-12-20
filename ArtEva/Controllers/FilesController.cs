using ArtEva.DTOs;
<<<<<<< HEAD
using ArtEva.Services.Interfaces;
=======
using ArtEva.Services;
using Microsoft.AspNetCore.Authorization;
>>>>>>> 7ef7d5956491c35f60b9324084ee1e37d86f8eee
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtEva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Seller")]

    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] FileUploadDto dto, [FromQuery] string type = "general")
        {
            var file = dto.File;
            var url = await _fileService.UploadImageAsync(file, type);
            return Ok(new { imageUrl = url });
        }

    }
}
