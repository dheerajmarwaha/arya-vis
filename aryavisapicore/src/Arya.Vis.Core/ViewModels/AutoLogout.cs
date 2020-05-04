using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.ViewModels
{
    public class AutoLogout
    {
        public bool? IsAutoLogoutEnabled { get; set; }
        public int? InactiveMinutes { get; set; }
    }
}
