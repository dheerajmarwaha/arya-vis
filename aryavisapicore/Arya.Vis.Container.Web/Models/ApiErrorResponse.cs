using Arya.Vis.Container.Web.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Models
{
    public class ApiErrorResponse
    {
        public ErrorBody Error { get; } = new ErrorBody();

        public ApiErrorResponse(string message, AppCode type)
        {
            Error = new ErrorBody { Message = message, Code = type.ToString() };
        }

        public ApiErrorResponse(string message, string code)
        {
            Error = new ErrorBody { Message = message, Code = code };
        }

        public ApiErrorResponse(string message, AppCode type, object info)
        {
            Error = new ErrorBody { Message = message, Code = type.ToString(), Info = info };
        }

        public ApiErrorResponse(string message, string code, object info)
        {
            Error = new ErrorBody { Message = message, Code = code, Info = info };
        }

        public class ErrorBody
        {
            public string Message { get; set; }
            public string Code { get; set; }
            public object Info { get; set; }
        }
    }
}
