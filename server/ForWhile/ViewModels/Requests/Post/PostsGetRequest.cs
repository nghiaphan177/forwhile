using ForWhile.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests
{
    public class PostsGetRequest : BasePageRequest
    {
        [Required(ErrorMessage = "Type is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TypeId must be a positive integer.")]
        public int TypeId { get; set; }

        public OrderType OrderBy { get; set; } = OrderType.MostVotes;

        public string Query { get; set; } = string.Empty;
    }
}
