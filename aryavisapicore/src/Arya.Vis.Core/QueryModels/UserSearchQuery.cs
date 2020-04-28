using System;
using System.Collections.Generic;
namespace Arya.Vis.Core.QueryModels
{
    public class UserSearchQuery : SimpleSearchQuery {
        public IEnumerable<string> VendorIds { get; set; }
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<string> Emails { get; set; }
        public IEnumerable<Guid> UserGuids { get; set; }
    }
}