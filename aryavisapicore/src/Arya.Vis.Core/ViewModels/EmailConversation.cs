using System;
using System.Collections.Generic;
namespace Arya.Vis.Core.ViewModels
{
    public class EmailConversation
    {
        public Guid? Id { get; set; }
        public IEnumerable<string> To { get; set; }
        public string From { get; set; }
        public Guid? TemplateId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsTestEmail { get; set; } = false;
        public Guid ConversationId { get; set; }
        public ConversationAutomationSettings AutomationSettings { get; set; }
    }
}