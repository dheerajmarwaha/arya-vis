using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.ViewModels
{
    public class OrganizationMetadataSearchResult
    {
        public IEnumerable<OrganizationMetadata> Organizations { get; set; }
        public int Total { get; set; }
    }
}
