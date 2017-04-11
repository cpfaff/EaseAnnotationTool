
using System.ComponentModel.DataAnnotations;

namespace CAFE.Web.Models.Account
{
    public class UserDetailsViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}