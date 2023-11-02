using Microsoft.AspNetCore.Http;

namespace RestaurantManagement.Domain.Helper
{
    public interface IImageHandler
    {
        Task SaveImage(string path, List<IFormFile> files, string defaultNameFile = null);
    }
}