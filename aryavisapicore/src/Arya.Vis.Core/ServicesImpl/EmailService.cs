using System;
using System.Threading.Tasks;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.ViewModels;
using Arya.Vis.Core.Utils;
using Arya.Vis.Core.Exceptions;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Arya.Exceptions;

namespace Arya.Vis.Core.ServicesImpl
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {
            
        }

        public async Task SendBulkAsync(int interviewId, BulkEmailConversationCommand command){

        }
        public async Task SendAsync(int interviewId, Guid candidateId, EmailConversation email){
            ValidationUtil.NotNull(email);            
           // await SendQuickConversationAsync(jobId, candidateId, email.To != null, JsonConvert.SerializeObject(email));
        }
        public async Task SendTestAsync(EmailConversation email){

        }

        private void ValidateBulkEmailConversationCommand(BulkEmailConversationCommand command) {
            ValidationUtil.NotNull(command);
            ValidationUtil.NotNull(command.EmailDetails);
            if (command.EmailDetails.TemplateId == null && string.IsNullOrWhiteSpace(command.EmailDetails.Body)) {
                throw new InvalidArgumentException($"{nameof(command.EmailDetails.TemplateId)}, {nameof(command.EmailDetails.Body)}", null, "Non-null and non-empty value of 'Body' or 'TemplateId' parameter.");
            }
            if (command.Candidates?.Any() != true) {
                throw new InvalidArgumentException(nameof(command.Candidates), null, "Non-null and non-empty value of 'Candidates' parameter.");
            }
            foreach (var candidateId in command.Candidates) {
                ValidationUtil.NotEmptyGuid(candidateId, nameof(candidateId));
            }
        }
    }
}