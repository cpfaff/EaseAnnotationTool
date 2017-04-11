using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CAFE.Web.Areas.Api.Models
{
    [DataContract]
    [JsonObject(IsReference = false)]
    public class GroupViewModel
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        //[JsonIgnore]
        //[IgnoreDataMember]
        [DataMember]
        public List<GroupsUserViewModel> Users { get; set; }
    }
}