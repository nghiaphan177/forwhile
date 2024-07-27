using System.ComponentModel.DataAnnotations;

namespace ForWhile.ViewModels.Requests
{
    public class AccountLoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
