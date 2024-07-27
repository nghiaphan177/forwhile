using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests.Comment
{
    public class CommentUpdateRequest
    {
        [Required(ErrorMessage = "Post ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Post ID must be a positive integer.")]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
    }
}
