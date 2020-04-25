using System;
using System.Threading.Tasks;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.ViewModels;
namespace Arya.Vis.Core.Services
{
    public interface IEmailService
    {
        Task SendBulkAsync(int interviewId, BulkEmailConversationCommand command);
        Task SendAsync(int interviewId, Guid candidateId, EmailConversation email);
        Task SendTestAsync(EmailConversation email);
    }
}