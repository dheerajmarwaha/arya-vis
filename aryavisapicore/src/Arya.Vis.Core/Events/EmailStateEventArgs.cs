using System;
using MailKit;

namespace Arya.Vis.Core.Events
{
    public class EmailStateEventArgs : EventArgs
    {
       public bool IsSuccess { get; set; }

        public MessageSentEventArgs args { get; set; }

        public EmailStateEventArgs(bool isSuccess, MessageSentEventArgs args)
        {
            IsSuccess = isSuccess;
            this.args = args;
        }
    }
}