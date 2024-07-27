using ForWhile.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests.Upvote
{
    public class UpvoteRequest
    {
        [Required]
        public UpvoteStatus Status { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PostId { get; set; }

        public int? CommentId { get; set; }
    }
}
