using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Entities
{
    public class Organization : OrganizationMetadata
    {
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string HomePageLink { get; set; }
        public string IdentityProviderIdentifier { get; set; }
        public string Address { get; set; }
        public int SourceLimit { get; set; }
        public int? JobCountLimit { get; set; }
        public string OrgLevelMiles { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}
