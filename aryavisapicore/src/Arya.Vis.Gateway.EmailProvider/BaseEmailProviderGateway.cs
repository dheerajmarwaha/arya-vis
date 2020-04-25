using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MailKit.Security;
using MailKit;
using MimeKit;
using MimeKit.Text;
using Arya.Vis.Core.Gateways;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Events;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;


namespace Arya.Vis.Gateway.EmailProvider
{
    public abstract class BaseEmailProviderGateway : IEmailProviderGateway
    {
        #region Events
        public delegate void EmailFailedEventHandler(object sender, Exception ex);
        public event EmailFailedEventHandler EmailFailed;
        public delegate void EmailSentEventHandler(object sender, EmailStateEventArgs e);
        public event EmailSentEventHandler EmailSent;
        public void OnEmailFailed(MimeMessage message, Exception ex)
        {
            EmailFailed?.Invoke(message, ex);
        }

        public void OnEmailSent(MimeMessage message, MessageSentEventArgs args)
        {
            EmailSent?.Invoke(message, new EmailStateEventArgs(true, args));
        }

        #endregion Events

        public string ProviderAuthScheme { get; }
        private readonly ILogger<BaseEmailProviderGateway> _logger;
        private const string MultiPartType = "mixed";

        private static readonly Regex _base64ImageRegex = new Regex("<img .*?src=\"data:(.*?)/(.*?);base64,(.*?)\".*?>");
        private static readonly Regex _whitespaceRegex = new Regex("\\s+");
        public BaseEmailProviderGateway(ILogger<BaseEmailProviderGateway> logger, string providerAuthScheme)
        {
            _logger = logger;
            ProviderAuthScheme = providerAuthScheme;
        }

        public abstract Task<EmailProviderResult> SendAsync(EmailRequest emailRequest,
                                                            Core.Entities.EmailProviderConfiguration providerConfiguration);


        protected MimeMessage CreateMessage(EmailRequest emailRequest)
        {
            if (!string.IsNullOrWhiteSpace(emailRequest.SubjectLine))
            {
                emailRequest.SubjectLine = _whitespaceRegex.Replace(emailRequest.SubjectLine, " ");
            }
            var message = new MimeMessage
            {
                Subject = emailRequest.SubjectLine,
            };

            message.From.AddRange(emailRequest.FromEmailAddress);

            message.To.AddRange(emailRequest.ToEmailAddresses);

            if (HasBase64Images(emailRequest.Body))
            {
                message.Body = ReplaceBase64Images(emailRequest.Body);
            }
            else
            {
                message.Body = new TextPart(TextFormat.Html) { Text = emailRequest.Body };
            }
            return message;
        }

        private Multipart ReplaceBase64Images(string body)
        {
            var matches = _base64ImageRegex.Matches(body);
            var parts = new List<MimePart>(matches.Count);
            var matchesCount = matches.Count;
            for (int i = matchesCount - 1; i >= 0; i--)
            {
                var match = matches[i];
                var imageBytes = Convert.FromBase64String(match.Groups[3].Value);
                var stream = new MemoryStream(imageBytes);
                // using(var stream = new MemoryStream(imageBytes)) {
                // stream.Seek(0, SeekOrigin.Begin);
                // var part = new LinkedResource(stream, match.Groups[1].Value);
                var contentId = Guid.NewGuid().ToString();
                var part = new MimePart(match.Groups[1].Value, match.Groups[2].Value)
                {
                    Content = new MimeContent(stream),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = contentId,
                    ContentId = contentId,
                    ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
                };
                parts.Add(part);
                body = $"{body.Substring(0, match.Index)}<img src=\"cid:{part.ContentId}\">{body.Substring(match.Index + match.Length)}";
                // }
            }
            var multipart = new Multipart(MultiPartType);
            multipart.Add(new TextPart(TextFormat.Html) { Text = body });
            foreach (var part in parts)
            {
                multipart.Add(part);
            }
            return multipart;
        }

        private bool HasBase64Images(string body)
        {
            return _base64ImageRegex.IsMatch(body);
        }
    }
}
