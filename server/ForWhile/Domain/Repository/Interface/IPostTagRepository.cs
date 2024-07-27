using ForWhile.Domain.Entities;
using System.Linq.Expressions;

namespace ForWhile.Domain.Repository.Interface
{
    public interface IPostTagRepository
    {
        Task<PostTag?> GetByIdAsync(int postId, int tagId, params Expression<Func<PostTag, object>>[] includeProperties);
        Task<IEnumerable<PostTag>> GetAllAsync(params Expression<Func<PostTag, object>>[] includeProperties);
        Task AddAsync(PostTag postTag);
        Task UpdateAsync(PostTag postTag);
        Task RemoveAsync(int postId, int tagId);
        Task RemoveRangeAsync(IEnumerable<PostTag> postTags);
        Task<bool> ExistsAsync(int postId, int tagId);
    }
}
