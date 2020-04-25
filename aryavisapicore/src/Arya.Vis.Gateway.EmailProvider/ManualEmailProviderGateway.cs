using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Config;
using Arya.Vis.Core.Gateways;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Arya.Vis.Gateway.EmailProvider.Repositories;
using System;

namespace Arya.Vis.Gateway.EmailProvider
{
    public class ManualEmailProviderGateway : BaseEmailProviderGateway
    {
        private readonly ILogger<ManualEmailProviderGateway> _logger;
        private static readonly string _providerAuthScheme = "Manual";
        public ManualEmailProviderGateway(
            ILogger<ManualEmailProviderGateway> logger
        ) : base(logger, _providerAuthScheme) {
            _logger = logger;
        }


        public override async Task<EmailProviderResult> SendAsync(EmailRequest emailRequest, 
                                                                  Core.Entities.EmailProviderConfiguration providerConfiguration) {
            _logger.LogInformation($"Started sending mail for To {string.Join(",",emailRequest.ToEmailAddresses.Select(to=> to.Address).ToString())} from ManualEmailProviderGateway");
             var messageToSend = CreateMessage(emailRequest);

            try{
                using(var smtpClient = new MailKit.Net.Smtp.SmtpClient()) {
                    
                    smtpClient.MessageSent += (sender, args) =>
                    {
                        OnEmailSent(messageToSend, args);
                    };

                    var manualConfiguration = providerConfiguration.Configuration as ManualEmailConfiguration;
                    if (manualConfiguration.EnableSsl != true) {
                        smtpClient.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; };
                    }
                    await smtpClient.ConnectAsync(manualConfiguration.SmtpServer, manualConfiguration.SmtpServerPort, SecureSocketOptions.StartTls);
                   
                    if (!string.IsNullOrWhiteSpace(manualConfiguration.SmtpUserName) &&
                        !string.IsNullOrWhiteSpace(manualConfiguration.SmtpPassword)) {
                        await smtpClient.AuthenticateAsync(manualConfiguration.SmtpUserName, manualConfiguration.SmtpPassword);
                    }
                    
                    var password = manualConfiguration.SmtpPassword;
                    manualConfiguration.SmtpPassword = null;
                    _logger.LogInformation("Sending email using manual email configuration {@ManualEmailConfiguration}", manualConfiguration);
                    manualConfiguration.SmtpPassword = password;
                   
                    await smtpClient.SendAsync(messageToSend);
                    await smtpClient.DisconnectAsync(true);
                    return null;
                }
            }
            //cattch(SmtpException){
            catch(Exception ex) { 
                OnEmailFailed(messageToSend, ex);
                return null;
            }         
        }      
        
    }
}