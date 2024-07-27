using ForWhile.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels
{
    public abstract class BasePageRequest
    {
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        [Required(ErrorMessage = "Page number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be a positive integer.")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Page size is required.")]
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 15;
    }
}
