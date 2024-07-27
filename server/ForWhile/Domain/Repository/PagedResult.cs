using ForWhile.Domain.Infrastructure;

namespace ForWhile.Domain.Repository
{
    public class PagedResult<T> where T : UniqueEntity
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
    }
}
