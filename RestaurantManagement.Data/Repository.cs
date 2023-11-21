using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace RestaurantManagement.Data
{
    public class Repository<T> where T : class
    {
        RestaurantManagementContext _restaurantManagementContext; //= database
        public Repository(RestaurantManagementContext restaurantManagementContext)
        {
            _restaurantManagementContext = restaurantManagementContext;
        }

        protected async Task<IEnumerable<T>> GetData(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return await _restaurantManagementContext.Set<T>().ToListAsync();
            }

            return await _restaurantManagementContext.Set<T>().Where(expression).ToListAsync();
        }

        protected async Task<T?> GetById(object id)
        {
            return await _restaurantManagementContext.Set<T>().FindAsync(id); //return { name: '', description: ''},
        }

        protected async Task<T?> GetSingleByConditionAsync(Expression<Func<T, bool>> expression = null)
        {
            return await _restaurantManagementContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        protected async Task Insert(T entity)
        {
            await _restaurantManagementContext.Set<T>().AddAsync(entity);
        }

        protected async Task Insert(IEnumerable<T> entities)
        {
            await _restaurantManagementContext.Set<T>().AddRangeAsync(entities);
        }

        protected void Update(T entity)
        {
            EntityEntry entityEntry = _restaurantManagementContext.Entry<T>(entity);
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        protected void Delete(T entity)
        {
            EntityEntry entityEntry = _restaurantManagementContext.Entry<T>(entity);
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        protected void Delete(Expression<Func<T, bool>> expression)
        {
            var entities = _restaurantManagementContext.Set<T>().Where(expression).ToList();
            if (entities.Count > 0)
            {
                _restaurantManagementContext.Set<T>().RemoveRange(entities);
            }
        }

        protected virtual IQueryable<T> Table => _restaurantManagementContext.Set<T>();

        protected async Task Commit()
        {
            await _restaurantManagementContext.SaveChangesAsync();
        }

       
    }
}
