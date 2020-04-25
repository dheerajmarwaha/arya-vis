using System;

namespace Arya.Vis.Core.Gateways
{
    public class ServiceProviderLog
    {
        public Guid PersonId { get; set; }
        public Guid ProviderId { get; set; }
        public Guid PayId { get; set; }
        public decimal CreditsCharged { get; set; }
        public string RawRequest { get; set; }
        public string RawResponse { get; set; }
        public ProviderRequestStatus RequestStatus { get; set; }
    }
}