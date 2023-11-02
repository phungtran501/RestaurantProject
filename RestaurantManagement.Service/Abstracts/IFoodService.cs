using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IFoodService
    {
        Task<ResponseModel> CreateUpdate(FoodModel foodModel);
        Task DeleteFood(int key);
        Task<ResponseDatatable> GetAllFood(RequestDatatable requestDatatable);
        Task<IEnumerable<FoodModel>> GetFoodByCategory(int categoryId);
        Task<FoodModel> GetFoodDetail(string code);
        Task<IEnumerable<FoodModel>> GetRandomFood();
    }
}