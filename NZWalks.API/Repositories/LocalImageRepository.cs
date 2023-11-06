using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext _context;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor
            ,NZWalksDbContext dbcontext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this._context = dbcontext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath,"Images",$"{image.FileName} {image.FileExtention}");
            //upload image to local file system
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //create variable or path for image
            //http://localhost:1234//images/image.jpg
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}" +
                              $"://{httpContextAccessor.HttpContext.Request.Host}" +
                              $"{httpContextAccessor.HttpContext.Request.PathBase}/Images/" +
                              $"{image.FileName}{image.FileExtention}";
            image.FilePath=urlFilePath;
            //add image to images table
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;


        }
    }
}
