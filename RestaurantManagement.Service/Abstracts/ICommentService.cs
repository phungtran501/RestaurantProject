using RestaurantManagement.UI.Areas.Admin.Models;

namespace RestaurantManagement.Service
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentModel>> GetCommentByFoodId(int foodId);
    }
}