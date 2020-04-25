using Arya.Vis.Core.Entities;

namespace Arya.Vis.Core.Config
{
    public class ProviderEmailConfig
    {
        public EmailAddress Email { get; set; }
        public bool? IsTestEmailSuccessfull { get; set; }
        public string Message { get; set; }
    }
}