using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IUserService
    {
        Task<ApplicationUser> CheckLogin(string username, string password);

    }
}