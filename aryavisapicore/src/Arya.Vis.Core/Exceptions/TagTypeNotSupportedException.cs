using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Exceptions
{
    public class TagTypeNotSupportedException : NotSupportedException
    {

        public TagTypeNotSupportedException(string unsupportedType, IEnumerable<string> supportedTypes) : base(GetMessage(unsupportedType, supportedTypes)) { }

        private static string GetMessage(string unsupportedType, IEnumerable<string> supportedTypes)
        {
            var commaSeperatedSupportedTypes = String.Join(",", supportedTypes);
            return $"Requested Tag Type: {unsupportedType} is not supported! Supported tag types are {commaSeperatedSupportedTypes}.";
        }
    }
}
