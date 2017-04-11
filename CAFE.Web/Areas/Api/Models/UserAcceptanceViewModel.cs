
using System;

namespace CAFE.Web.Areas.Api.Models
{
    public class UserAcceptanceViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public bool IsAccepted { get; set; }

    }
}