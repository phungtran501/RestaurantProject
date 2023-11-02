using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Data.Abstract
{
    public interface IRepository<T> where T : class
    {
        Task Commit();
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> expression);
        Task<T?> GetById(object id);
        Task<IEnumerable<T>> GetData(Expression<Func<T, bool>> expression = null);
        Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression = null);
        Task Insert(T entity);
        Task Insert(IEnumerable<T> entities);
        void Update(T entity); 
    }
}
