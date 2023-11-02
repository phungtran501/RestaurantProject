namespace RestaurantManagement.UI.Helper
{
    public interface IImageHandler
    {
        Task SaveImage(string path, List<IFormFile> files, string defaultNameFile = null);
    }
}