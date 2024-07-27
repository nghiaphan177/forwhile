using ForWhile.Domain.Enums;
using ForWhile.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForWhile.Domain.Entities
{
    [Table("Upvote")]
    public class Upvote : UniqueEntity
    {
        [Required]
        public UpvoteStatus Status { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Comment")]
        public int? CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
