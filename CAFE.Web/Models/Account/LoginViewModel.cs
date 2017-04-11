
using System.ComponentModel.DataAnnotations;

namespace CAFE.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}