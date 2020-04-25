using System;

namespace Arya.Vis.Core.Entities
{
    public class EmailAddress
    {
        public Guid? Id { get; set; }
        public string EmailId { get; set; }
        public EmailVerificationStatus? VerificationStatus { get; set; }
    }
}