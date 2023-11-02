using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Service
{
    public class UserTokenService : IUserTokenService
    {
        IUnitOfWork _unitOfWork;

        public UserTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SaveToken(UserToken userToken)
        {
            await _unitOfWork.UserTokenRepository.Insert(userToken);
            await _unitOfWork.UserTokenRepository.Commit();
        }

        public async Task<UserToken> CheckRefreshToken(string code)
        {
            return await _unitOfWork.UserTokenRepository.GetSingleByConditionAsync(x => x.CodeRefreshToken == code);
        }
    }
}
