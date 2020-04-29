using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Entities
{
    public class OrganizationMetadata
    {      
        public Guid OrgGuid { get; set; }
        public string OrganizationName { get; set; }
        public string OrgType { get; set; }
        public string OrgCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public Guid CreatedByGuId { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid ModifiedByGuId { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
