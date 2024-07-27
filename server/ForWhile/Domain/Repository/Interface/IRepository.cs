using ForWhile.Domain.Enums;
using ForWhile.Domain.Infrastructure;
using System.Linq.Expressions;

namespace ForWhile.Domain.Repository.Interface
{
    public interface IRepository<T> where T : UniqueEntity
    {
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties);

        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<PagedResult<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy,
            SortDirection sortDirection, int pageIndex, int pageSize, params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task RemoveAsync(T entity);

        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
