using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IMenuDetailService
    {
        Task DeleteMenuDetail(int key);
        Task<IEnumerable<MenuDetailModel>> GetFoodByMenu(int idMenu);
        Task InsertUpdate(List<MenuDetailModel> menuDetailModel);
    }
}