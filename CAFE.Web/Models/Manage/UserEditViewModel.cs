
using System.ComponentModel;

namespace CAFE.Web.Models.Manage
{
    public class UserEditViewModel
    {
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Salutation { get; set; }
        [Description("Postal address")]
        public string PostalAddress { get; set; }
        [Description("Old password")]
        public string OldPassword { get; set; }
        [Description("New password")]
        public string NewPassword { get; set; }
        [Description("Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}