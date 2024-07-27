using ForWhile.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests.Comment
{
    public class CommentsGetRequest : BasePageRequest
    {
        [Required]
        public int PostId { get; set; }

        public OrderType OrderBy { get; set; }
    }
}
