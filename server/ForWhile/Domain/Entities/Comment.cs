using ForWhile.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForWhile.Domain.Entities
{
    [Table("Comment")]
    public class Comment : UniqueEntity
    {
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [ForeignKey("User")]
        public int AuthorId { get; set; }
        public User Author { get; set; }


        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        public ICollection<Upvote> Upvotes { get; set; } = new List<Upvote>();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
