using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service
{
    public class MenuDetailService : IMenuDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFoodService _foodService;
        private readonly IMenuService _menuService;

        public MenuDetailService(IUnitOfWork unitOfWork, IMenuService menuService, IFoodService foodService)
        {
            _unitOfWork = unitOfWork;
            _foodService = foodService;
            _menuService = menuService;
        }

        public async Task<IEnumerable<MenuDetailModel>> GetFoodByMenu(int idMenu)
        {
            var menu = await _unitOfWork.MenuDetailRepository.GetData(x => x.MenuId == idMenu);

            var getMenuDetail = menu.Select(x => new MenuDetailModel
            {
                FoodId = x.FoodId,
                Price = x.Price,
                Id = x.Id,
            });

            return getMenuDetail;
        }

        public async Task DeleteMenuDetail(int key)
        {
            var food = await _unitOfWork.MenuDetailRepository.GetById(key);
            _unitOfWork.MenuDetailRepository.Delete(food);
            await _unitOfWork.MenuDetailRepository.Commit();
        }

        public async Task InsertUpdate(List<MenuDetailModel> menuDetailModel)
        {
            foreach (var item in menuDetailModel)
            {
                if (item.Id == 0)
                {
                    var food = new MenuDetail
                    {
                        FoodId = item.FoodId,
                        Price = item.Price,
                        MenuId = item.MenuId
                    };

                    await _unitOfWork.MenuDetailRepository.Insert(food);

                }
                //update
                else
                {
                    var food = await _unitOfWork.MenuDetailRepository.GetById(item.Id);
                    food.FoodId = item.FoodId;
                    food.Price = item.Price;
                    
                    _unitOfWork.MenuDetailRepository.Update(food);
                }
            }
            await _unitOfWork.MenuDetailRepository.Commit();

        }
    }
}
