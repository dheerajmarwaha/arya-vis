using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DatabaseMapAttribute : Attribute
    {
        public string DBColumnName { get; set; }
        public DatabaseMapAttribute(string name)
        {
            this.DBColumnName = name;
        }
    }
}
