using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Security
{
    public class SecurityRoleRequirement : IAuthorizationRequirement
    {
        public string SecurityRole { get; set; }
        public SecurityRoleRequirement(string securityRole)
        {
            SecurityRole = securityRole;
        }
    }
}
