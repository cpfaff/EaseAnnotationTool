
using System.ComponentModel.DataAnnotations;

namespace CAFE.Web.Models.Account
{
    public class UserProfileViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Salutation { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
    }
}