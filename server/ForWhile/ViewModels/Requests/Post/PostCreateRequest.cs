using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests
{
    public class PostCreateRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "AuthorID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "AuthorID must be a positive integer.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "TypeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TypeId must be a positive integer.")]
        public int TypeId { get; set; }

        public List<string>? Tags { get; set; }
    }
}
