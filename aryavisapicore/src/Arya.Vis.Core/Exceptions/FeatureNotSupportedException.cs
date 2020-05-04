using Arya.Vis.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Exceptions
{
    public class FeatureNotSupportedException : NotSupportedException
    {
        public FeatureNotSupportedException(FeatureName feature) : base(GetMessage(feature)) { }

        private static string GetMessage(FeatureName feature)
        {
            return $"Requested feature: {feature} is not supported!";
        }
    }
}
