using System;
using System.Collections.Generic;
using MimeKit;
/* @author Dheeraj Marwaha */

namespace Arya.Vis.Core.Entities
{  
    public class EmailRequest
    {
        public List<MailboxAddress> ToEmailAddresses {get;set;}
        public List<MailboxAddress> FromEmailAddress {get;set;}
        public List<MailboxAddress> CCToEmailAddresses {get;set;}

        public string SubjectLine { get; set; }
        public string Body { get; set; }
        public DateTime? SentTime { get; set; }
        public DateTime? ViewedTime { get; set; }
        public DateTime? ClickedTime { get; set; }
        public bool IsTestEmail { get; set; } = false;
    }
}