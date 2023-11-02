using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Service.Abstracts
{
    public interface IUserTokenService
    {
        Task<UserToken> CheckRefreshToken(string code);
        Task SaveToken(UserToken userToken);
    }
}