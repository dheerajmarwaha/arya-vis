using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Exceptions
{
    public class LimitExceededException : Exception
    {
        public string Action { get; private set; }
        public int Limit { get; private set; }

        public LimitExceededException(string action, int limit) : base(GetMessage(action, limit))
        {
            Action = action;
            Limit = limit;
        }

        public LimitExceededException(string action, int limit, Exception innerException) : base(GetMessage(action, limit), innerException)
        {
            Action = action;
            Limit = limit;
        }

        private static string GetMessage(string action, int limit)
        {
            var errorMessage = $"Limit exceeded for action: {action}. Max limit allowed: {limit}";
            return errorMessage;
        }

    }
}
