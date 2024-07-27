using ForWhile.Domain.Entities;
using ForWhile.Domain.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForWhile.Domain.Repository
{
    public class PostTagRepository : IPostTagRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PostTagRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PostTag?> GetByIdAsync(int postId, int tagId, params Expression<Func<PostTag, object>>[] includeProperties)
        {
            IQueryable<PostTag> items = _dbContext.PostTags;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return await items.FirstOrDefaultAsync(pt => pt.PostId == postId && pt.TagId == tagId);
        }

        public async Task<IEnumerable<PostTag>> GetAllAsync(params Expression<Func<PostTag, object>>[] includeProperties)
        {
            IQueryable<PostTag> items = _dbContext.PostTags;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return await items.ToListAsync();
        }

        public async Task AddAsync(PostTag postTag)
        {
            _dbContext.PostTags.Add(postTag);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(PostTag postTag)
        {
            _dbContext.PostTags.Update(postTag);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(int postId, int tagId)
        {
            var postTag = await GetByIdAsync(postId, tagId);
            if (postTag != null)
            {
                _dbContext.PostTags.Remove(postTag);
                await _dbContext.SaveChangesAsync();
            }
        }
        public virtual async Task RemoveAsync(PostTag postTag)
        {
            _dbContext.PostTags.Remove(postTag);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<PostTag> postTags)
        {
            _dbContext.PostTags.RemoveRange(postTags);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int postId, int tagId)
        {
            return await _dbContext.PostTags.AnyAsync(pt => pt.PostId == postId && pt.TagId == tagId);
        }
    }
}
