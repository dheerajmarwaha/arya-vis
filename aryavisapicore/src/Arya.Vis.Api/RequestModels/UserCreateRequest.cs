using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.RequestModels
{
    public class UserCreateRequest
    {
        public User User { get; set; }
        public string Password { get; set; }
    }
}
