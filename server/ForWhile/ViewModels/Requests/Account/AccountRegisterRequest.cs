using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests.Account
{
    public class AccountRegisterRequest
    {
        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        public string? Password { get; set; }
    }
}
