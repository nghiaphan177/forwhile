using ForWhile.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForWhile.Domain.Entities
{
    [Table("Tag")]
    public class Tag : UniqueEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int Count { get; set; }

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
