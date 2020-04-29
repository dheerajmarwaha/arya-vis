using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Exceptions
{
    [Serializable]
    public class IdentityProviderCodeConfirmationFailedException : Exception
    {
        public string Reason { get; set; }
        private static string GetMessage(string reason)
        {
            return $"Code Confirmation failed due to {reason}";
        }

        public IdentityProviderCodeConfirmationFailedException(string reason, Exception inner) : base(GetMessage(reason), inner)
        {
            Reason = reason;
        }
    }
}
