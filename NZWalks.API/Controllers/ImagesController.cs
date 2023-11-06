using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //post api/images/upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileExtension(request);
            if (ModelState.IsValid)
            {
                //convert Dto to domain model
                var imageDomainModel =new Image()
                {
                    File=request.File,
                    FileName=request.FileName,
                    FileDescription=request.FileDescription,
                    FileExtention=Path.GetExtension(request.File.FileName),
                    FileSizeInBytes=request.File.Length,
                    FilePath=Path.Combine("Uploads",request.File.FileName)
                };
                //user repository to upload image to database
                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest("Invalid file extension");
        }
        //private method to check valid file extension
        private void ValidateFileExtension(ImageUploadRequestDTO request)
        {
            //get file extension
            var allowedExtension = new string[] { ".jpg", ".png", ".jpeg" };
            if(!allowedExtension.Contains(Path.GetExtension(request.File.FileName).ToLower()))
            {
                ModelState.AddModelError("File", "Invalid file extension");
            }
            if(request.File.Length>10485760)
            {
                ModelState.AddModelError("File", "File size can not be more than 1MB");
            }
        }

    }
}
