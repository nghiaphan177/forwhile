using ForWhile.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForWhile.Domain.Entities
{
    [Table("Post")]

    public class Post : UniqueEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("User")]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public PostType Type { get; set; }
        public int View { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Upvote> Upvotes { get; set; } = new List<Upvote>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

    }
}
