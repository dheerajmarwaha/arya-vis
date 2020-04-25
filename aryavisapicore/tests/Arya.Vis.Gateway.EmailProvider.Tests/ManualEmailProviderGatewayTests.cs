using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using MailKit;
using MimeKit;
using Arya.Vis.Core.Config;
using Arya.Vis.Core.Entities;
using System.Net.Sockets;

namespace Arya.Vis.Gateway.EmailProvider.Tests
{
    public class ManualEmailProviderGatewayTests
    {
        private readonly Mock<ILogger<ManualEmailProviderGateway>> _logger;
        private readonly ManualEmailProviderGateway _gateway;

        private EmailProviderConfiguration providerConfiguration;
        private EmailRequest emailRequest;
        public ManualEmailProviderGatewayTests()
        {
            _logger = new Mock<ILogger<ManualEmailProviderGateway>>();
            _gateway = new ManualEmailProviderGateway(_logger.Object);

            //Event bindings
            _gateway.EmailSent += _gateway_EmailSent;
            _gateway.EmailFailed += _gateway_EmailFailed;
        }
        
        [Fact]
        public void SendAsync_SingleRecipient_ShouldPass()
        {
            Initialize();  

            _gateway.SendAsync(emailRequest, providerConfiguration).Wait();
        }

        [Fact]
        public void SendAsync_InvalidHost_ShouldPass()
        {
            Initialize(smtpServer: "dummy.outlook.com");
           
            _gateway.EmailFailed += _gateway_EmailFailed_NoHost;
            _gateway.SendAsync(emailRequest, providerConfiguration).Wait();
        }

        [Fact]
        public  void SendAsync_InvalidRecipient_ShouldPass()
        {
            Initialize(emailTo:"invalidEmail@gmail.com");

            var result = _gateway.SendAsync(emailRequest, providerConfiguration).Result;
            Assert.Null(result);
        }




        private void _gateway_EmailFailed_NoHost(object sender, Exception ex)
        {
            var message = sender as MimeMessage;
            Assert.Equal("No such host is known.", ex.Message);
        }

        private void _gateway_EmailFailed(object sender, Exception ex)
        {
            var message = sender as MimeMessage;
        }

        private void _gateway_EmailSent(object sender, Core.Events.EmailStateEventArgs e)
        {
            var message = sender as MimeMessage;
        }

        private void Initialize(string smtpServer = "smtp-mail.outlook.com", string emailTo = "dheerajmarwaha@gmail.com")
        {
            providerConfiguration = new EmailProviderConfiguration()
            {
                Configuration = new ManualEmailConfiguration()
                {
                    SmtpServer = smtpServer,
                    SmtpServerPort = 587,
                    SmtpUserName = "bcc.bangalore@outlook.com",
                    SmtpPassword = "Dhe@2498214"
                }
            };

            emailRequest = new EmailRequest
            {
                ToEmailAddresses = new List<MailboxAddress>() { new MailboxAddress("Dheeraj Marwaha", emailTo) },
                FromEmailAddress = new List<MailboxAddress>() { new MailboxAddress("Leoforce Test System", "bcc.bangalore@outlook.com") },
                SubjectLine = "Testing Mail service",
                Body = @$"<h1>Dear <i>Candidate</i></h1><br/>                         
                        <br/>This is a dummy mail. Feel free to reach us at test@leoforce.com 
                        <br/> 
                        <br/><br/>Regards,
                        <br/>Test System"
            };
        }
    }
}
