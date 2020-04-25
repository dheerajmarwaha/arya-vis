using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Arya.Vis.Core.Exceptions
{
    public class InvalidArgumentException : Exception {

        public string ParamName { get; }
        public string ParamValue { get; }
        public string ParamExpectedValue { get; }

        public InvalidArgumentException(string paramName, string paramValue, string paramExpectedValue):
            base(GetMessage(paramName, paramValue, paramExpectedValue)) {
                ParamName = paramName;
                ParamValue = paramValue;
                ParamExpectedValue = paramExpectedValue;
            }

        public InvalidArgumentException(string paramName, string paramValue, string paramExpectedValue, string message):
            base(GetMessage(paramName, paramValue, paramExpectedValue, message)) {
                ParamName = paramName;
                ParamValue = paramValue;
                ParamExpectedValue = paramExpectedValue;
            }

        public InvalidArgumentException(string paramName, string paramValue, string paramExpectedValue, string message, Exception innerException):
            base(GetMessage(paramName, paramValue, paramExpectedValue, message), innerException) {
                ParamName = paramName;
                ParamValue = paramValue;
                ParamExpectedValue = paramExpectedValue;
            }

        private static string GetMessage(string paramName, string paramValue, string paramExpectedValue, string message = null) {
            var errorMessage = $"Invalid argument value: {paramValue} supplied for argument: {paramName}";
            if (!string.IsNullOrWhiteSpace(paramExpectedValue)) {
                errorMessage += $"; expected argument value: {paramExpectedValue}";
            }
            if (!string.IsNullOrWhiteSpace(message)) {
                errorMessage += "; " + message;
            }
            return errorMessage;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected InvalidArgumentException(SerializationInfo info, StreamingContext context):
            base(info, context) {
                ParamName = info.GetString("ParamName");
                ParamValue = info.GetString("ParamValue");
                ParamExpectedValue = info.GetString("ParamExpectedValue");
            }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("ParamName", ParamName);
            info.AddValue("ParamValue", ParamValue);
            info.AddValue("ParamExpectedValue", ParamExpectedValue);
            base.GetObjectData(info, context);
        }
    }

}