using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IUserAddressService
    {
        Task<ResponseModel> CreateUpdate(UserAddressViewModel userAddressViewModel);
        Task DeleteUserAddress(int key);
        Task<ResponseDatatable> GetAllUserAddress(RequestDatatable requestDatatable);
    }
}