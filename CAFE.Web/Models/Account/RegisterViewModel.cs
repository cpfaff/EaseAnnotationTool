
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;

namespace CAFE.Web.Models.Account
{
	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
		[DataType(DataType.Text)]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
		[DataType(DataType.Text)]
		[Display(Name = "Surname")]
		public string Surname { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}