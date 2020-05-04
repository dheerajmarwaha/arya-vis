using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso2Code { get; set; }
        public string Iso3Code { get; set; }
        public bool IsSubscribed { get; set; }
        public LocationStatsLevel? StatsLevel { get; set; }
        public int RegionId { get; set; }
        public bool IsActive { get; set; }
    }
}
