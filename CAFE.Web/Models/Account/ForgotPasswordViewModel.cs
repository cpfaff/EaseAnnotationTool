
using System.ComponentModel.DataAnnotations;

namespace CAFE.Web.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email or Login name")]
        public string EmailOrUserName { get; set; }
    }
}