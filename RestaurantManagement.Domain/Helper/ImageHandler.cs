using Microsoft.AspNetCore.Http;

namespace RestaurantManagement.Domain.Helper
{
    public class ImageHandler : IImageHandler
    {
        public async Task SaveImage(string path, List<IFormFile> files, string defaultNameFile = null)
        {
            if (files.Any())
            {
                string basePath = Path.Combine(Directory.GetCurrentDirectory(), path); 

                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                foreach (var file in files)
                {
                    if (file is not null)
                    {
                        basePath = Path.Combine(basePath, !string.IsNullOrEmpty(defaultNameFile) ? defaultNameFile : file.Name);
                        using (var fileStream = new FileStream(basePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }


                }
            }
        }
    }
}
