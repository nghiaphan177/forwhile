using ForWhile.Domain.Enums;
using ForWhile.Domain.Infrastructure;
using ForWhile.Domain.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForWhile.Domain.Repository
{
    public class Repository<T> : IRepository<T> where T : UniqueEntity
    {
        protected readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetAll(includeProperties).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            var query = await GetAllAsync(predicate);
            return query.SingleOrDefault();
        }

        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {

            return await GetAll(includeProperties).Where(predicate).ToListAsync();
        }

        public virtual async Task<PagedResult<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy,
            SortDirection sortDirection, int pageIndex, int pageSize, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            // Total page
            int totalPage = (await query.CountAsync(predicate)) / pageSize + 1;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            var result = new PagedResult<T> { TotalPages = totalPage };

            if (sortDirection == SortDirection.Ascending)
            {
                result.Items = await query.Where(predicate).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                result.Items = await query.Where(predicate).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return result;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}
