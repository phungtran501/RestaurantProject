using Dapper;
using System.Data;

namespace RestaurantManagement.Data.Abstract
{
    public interface IDapperHelper
    {
        Task<IEnumerable<T>> ExcuteStoreProcedureReturnList<T>(string query, DynamicParameters parameters, IDbTransaction dbTransaction = null);
        Task ExecuteNotReturn(string query, DynamicParameters parameters, IDbTransaction dbTransaction = null);
        Task<T?> ExecuteReturnScalarAsync<T>(string query, DynamicParameters parameters, IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteSqlReturnList<T>(string query, DynamicParameters parameters, IDbTransaction dbTransaction = null);
    }
}