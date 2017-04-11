
using System.ComponentModel;

namespace CAFE.Web.Models.Manage
{
    public class UserDetailsViewModel
    {
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Salutation { get; set; }
        [Description("Postal address")]
        public string PostalAddress { get; set; }
    }
}