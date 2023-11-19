using Dapper;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Enums;
using RestaurantManagement.Service.Abstracts;
using RestaurantManagement.Service.DTOs;
using RestaurantManagement.UI.Areas.Admin.Models;
using System.Data;

namespace RestaurantManagement.Service
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperHelper _dapperHelper;

        public UserAddressService(IUnitOfWork unitOfWork, IDapperHelper dapperHelper)
        {
            _unitOfWork = unitOfWork;
            _dapperHelper = dapperHelper;
        }

        public async Task<ResponseDatatable> GetAllUserAddress(RequestDatatable requestDatatable)
        {
            ResponseDatatable responseDatatable;

            try
            {
                var users = await _unitOfWork.UserAddressRepository.GetData(f => f.IsActive);

                DynamicParameters dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("skipItem", requestDatatable.SkipItems, DbType.Int32, ParameterDirection.Input);
                dynamicParameters.Add("pageSize", requestDatatable.PageSize, DbType.Int32, ParameterDirection.Input);
                dynamicParameters.Add("keyWord", requestDatatable.Keyword, DbType.String, ParameterDirection.Input);
                dynamicParameters.Add("totalRecord", 0, DbType.Int32, ParameterDirection.Output);

                var result = await _dapperHelper.ExcuteStoreProcedureReturnList<UserAddressModel>("GetAllUserAddress", dynamicParameters);

                var total = dynamicParameters.Get<int>("totalRecord");

                var data = result.Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Fullname,
                    x.Address,
                    x.Phone,
                    x.IsActive

                }).ToArray();

                responseDatatable = new ResponseDatatable
                {
                    Draw = requestDatatable.Draw,
                    RecordsTotal = users.Count(),
                    RecordsFiltered = users.Count(),
                    Data = data
                };
            }
            catch (Exception ex)
            {
                throw;
            }

            return responseDatatable;
        }

        public async Task<ResponseModel> CreateUpdate(UserAddressViewModel userAddressViewModel)
        {

            if (userAddressViewModel.Id == 0)
            {

                var userAddress = new UserAddress
                {
                    Address = userAddressViewModel.Address,
                    Phone = userAddressViewModel.Phone,
                    IsActive = userAddressViewModel.IsActive,
                    Fullname = userAddressViewModel.Fullname,
                    UserId = userAddressViewModel.UserId
                };

                var result = _unitOfWork.UserAddressRepository.Insert(userAddress);

                await _unitOfWork.UserAddressRepository.Commit();

            }
            //update
            else
            {
                var userAddress = await _unitOfWork.UserAddressRepository.GetById(userAddressViewModel.Id);

                userAddress.Phone = userAddressViewModel.Phone;
                userAddress.IsActive = userAddressViewModel.IsActive;
                userAddress.Address = userAddressViewModel.Address;
                userAddress.Fullname = userAddressViewModel.Fullname;

                _unitOfWork.UserAddressRepository.Update(userAddress);
                await _unitOfWork.UserAddressRepository.Commit();
            }

            return new ResponseModel
            {
                Status = true,
                Message = userAddressViewModel.Id == 0 ? "Insert successful" : "Update successful",
                StatusType = StatusType.Success,
                Action = userAddressViewModel.Id == 0 ? ActionType.Insert : ActionType.Update
            };

        }

        public async Task DeleteUserAddress(int key)
        {
            var userAddress = await _unitOfWork.UserAddressRepository.GetById(key);
            userAddress.IsActive = false;
            _unitOfWork.UserAddressRepository.Update(userAddress);
            await _unitOfWork.UserAddressRepository.Commit();
        }
    }
}
