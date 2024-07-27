using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests
{
    public class PostUpdateRequest
    {
        [Required(ErrorMessage = "Post ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Post ID must be a positive integer.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        public List<string>? Tags { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
