using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests.Comment
{
    public class CommentCreateRequest
    {
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        public int AuthorID { get; set; }

        public int PostID { get; set; }
    }
}
