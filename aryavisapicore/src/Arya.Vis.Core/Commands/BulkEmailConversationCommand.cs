using System;
using System.Collections.Generic;
using Arya.Vis.Core.ViewModels;
namespace Arya.Vis.Core.Commands
{
    public class BulkEmailConversationCommand
    {
        public EmailConversation EmailDetails { get; set; }
        public IEnumerable<Guid> Candidates { get; set; }
    }
}