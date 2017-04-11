
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CAFE.Web.Areas.Api.Models
{
    [DataContract]
    [JsonObject(IsReference = true)]
    public class UserViewModel
    {
        public UserViewModel()
        {
            Role = "Administrator";
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string PostalAddress { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string Salutation { get; set; }

        [DataMember]
        public bool IsAccepted { get; set; }

        [DataMember]
        public string Role { get; set; }

        //[JsonIgnore] 
        //[IgnoreDataMember]
        [DataMember]
        public List<UserGroupsViewModel> Groups { get; set; }
    }
}