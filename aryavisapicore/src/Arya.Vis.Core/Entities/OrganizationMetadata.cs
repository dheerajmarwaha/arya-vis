using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Entities
{
    public class OrganizationMetadata
    {      
        public Guid OrgGuId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationType { get; set; }
        public bool IsActive { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
