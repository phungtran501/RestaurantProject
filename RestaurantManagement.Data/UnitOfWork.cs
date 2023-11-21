using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        RestaurantManagementContext _restaurantManagementContext;

        Repository<Menu> _repositoryMenu;
        Repository<Cart> _repositoryCart;
        Repository<UserAddress> _repositoryUserAddress;
        Repository<MenuDetail> _repositoryMenuDetail;
        Repository<CartDetail> _repositoryCartDetail;
        Repository<Category> _repositoryCategory;
        Repository<Comment> _repositoryComment;
        Repository<Food> _repositoryFood;

        private bool disposedValue;

        public UnitOfWork(RestaurantManagementContext restaurantManagementContext)
        {
            _restaurantManagementContext = restaurantManagementContext;
        }

        public Repository<Menu> MenuRepository => _repositoryMenu ??= new Repository<Menu>(_restaurantManagementContext);

        public Repository<Cart> CartRepository => _repositoryCart ??= new Repository<Cart>(_restaurantManagementContext);


        public Repository<UserAddress> UserAddressRepository => _repositoryUserAddress ??= new Repository<UserAddress>(_restaurantManagementContext);

        public Repository<MenuDetail> MenuDetailRepository => _repositoryMenuDetail ??= new Repository<MenuDetail>(_restaurantManagementContext);


        public Repository<CartDetail> CartDetailRepository => _repositoryCartDetail ??= new Repository<CartDetail>(_restaurantManagementContext);

        public Repository<Category> CategoryRepository => _repositoryCategory ??= new Repository<Category>(_restaurantManagementContext);

        public Repository<Comment> CommentRepository => _repositoryComment ??= new Repository<Comment>(_restaurantManagementContext);

        public Repository<Food> FoodRepository => _repositoryFood ??= new Repository<Food>(_restaurantManagementContext);

        public Repository<UserToken> UserTokenRepository => _repositoryUserToken ??= new Repository<UserToken>(_restaurantManagementContext);


        public async Task CommitAsync()
        {
            await _restaurantManagementContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if(_restaurantManagementContext != null)
            {
                _restaurantManagementContext.Dispose();
            }    
        }
    }
}
