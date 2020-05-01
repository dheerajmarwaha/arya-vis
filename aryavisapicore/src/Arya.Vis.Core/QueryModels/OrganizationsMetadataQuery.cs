using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.QueryModels
{
    public class OrganizationsMetadataQuery : PaginatedQuery
    {
        public string SearchKeyword { get; set; }
        public OrganizationsSortField SortBy { get; set; } = OrganizationsSortField.OrganizationName;
        public SortOrder SortOrder { get; set; } = SortOrder.Descending;
    }
}
