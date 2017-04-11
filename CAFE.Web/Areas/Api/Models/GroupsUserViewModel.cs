
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CAFE.Web.Areas.Api.Models
{
    [DataContract]
    [JsonObject(IsReference = false)]
    public class GroupsUserViewModel
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
    }
}