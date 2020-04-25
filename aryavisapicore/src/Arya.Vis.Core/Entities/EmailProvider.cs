using System;
using System.Collections.Generic;
using Arya.Vis.Core.Config;
using Arya.Vis.Core.Services;

namespace Arya.Vis.Core.Entities
{
    public class EmailProviderConfiguration
    {
         public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public string AuthScheme { get; set; }
        public IEmailConfiguration Configuration { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool? IsActive { get; set; }
        public IEnumerable<ProviderEmailConfig> Emails { get; set; }
    }
}