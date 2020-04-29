using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.RequestModels
{
    public class UserVerificationRequest
    {
        public Guid UserGuid { get; set; }
        public string VerificationCode { get; set; }
    }
}
