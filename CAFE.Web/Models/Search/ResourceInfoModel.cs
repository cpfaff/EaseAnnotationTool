
using CAFE.Core.Searching;
using System;
using System.Collections.Generic;

namespace CAFE.Web.Models.Search
{
    public class ResourceInfoModel
    {
        public Guid Id { get; set; }
        public SearchResultItemType ResourceType { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string OwnerName { get; set; }
        public string OwnerId { get; set; }
        public bool HasAccess { get; set; }
        public string Link { get; set; }

        public List<string> Hosters { get; set; } = new List<string>();
        public List<PersonModel> Persons { get; set; } = new List<PersonModel>();

    }

    public class PersonModel
    {
        public string Position { get; set; }
        public string Salutation { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}

