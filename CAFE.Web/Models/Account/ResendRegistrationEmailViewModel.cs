
using System.ComponentModel.DataAnnotations;

namespace CAFE.Web.Models.Account
{
    public class ResendRegistrationEmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}