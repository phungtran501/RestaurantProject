using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Service
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CommentModel>> GetCommentByFoodId(int foodId)
        {
            //var comments = await _unitOfWork.CommentRepository.GetData(x => x.FoodId == id);
            List<ApplicationUser> users = await _userManager.Users.ToListAsync();

            var comments = await _unitOfWork.CommentRepository.GetData(x => x.IsActive);

            var getComments = from c in comments
                              join app in users
                           on c.UserId equals app.Id
                              where c.FoodId == foodId
                              select new CommentModel
                              {
                                  Content = c.Content,
                                  Id = c.Id,
                                  CreatedOn = c.CreatedOn,
                                  UserName = app.UserName
                              };


            return getComments;

        }
    }
}
