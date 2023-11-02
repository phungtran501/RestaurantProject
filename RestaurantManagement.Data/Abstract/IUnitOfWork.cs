using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Data.Abstract
{
    public interface IUnitOfWork
    {
        Repository<CartDetail> CartDetailRepository { get; }
        Repository<Cart> CartRepository { get; }
        Repository<Category> CategoryRepository { get; }
        Repository<Comment> CommentRepository { get; }
        Repository<Food> FoodRepository { get; }
        Repository<MenuDetail> MenuDetailRepository { get; }
        Repository<Menu> MenuRepository { get; }
        Repository<UserAddress> UserAddressRepository { get; }
        Repository<UserToken> UserTokenRepository { get; }

        Task CommitAsync();
        void Dispose();
    }
}