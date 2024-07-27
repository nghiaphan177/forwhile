using ForWhile.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForWhile.Domain.Entities
{
    [Table("PostType")]

    public class PostType : UniqueEntity
    {
        [Required]
        [MaxLength(200)]
        public string TypeName { get; set; }

    }
}
