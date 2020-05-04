using Arya.Vis.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Entities
{
    public class Feature
    {
        public FeatureName Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsAllowed { get; set; }
    }
}
