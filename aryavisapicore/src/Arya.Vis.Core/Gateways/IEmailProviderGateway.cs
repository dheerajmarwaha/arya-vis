using System.Threading.Tasks;
using Arya.Vis.Core.Entities;

/* @author Mouli Kalakota */

namespace Arya.Vis.Core.Gateways
{
    public interface IEmailProviderGateway
    {
        string ProviderAuthScheme { get; }
        Task<EmailProviderResult> SendAsync(EmailRequest emailRequest, EmailProviderConfiguration providerConfiguration);
    }
}