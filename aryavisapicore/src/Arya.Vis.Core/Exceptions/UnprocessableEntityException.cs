using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public string EntityName { get; private set; }
        public object RequestedParams { get; private set; }
        public UnprocessableEntityException() { }
        public UnprocessableEntityException(string entityName, object requestedParams) : base(GetMessage(requestedParams))
        {
            EntityName = entityName;
            RequestedParams = requestedParams;
        }

        private static string GetMessage(object requestedParams)
        {
            var errorMessage = $"Unable to process request with given parameters : {JsonConvert.SerializeObject(requestedParams)}";
            return errorMessage;
        }
    }
}
