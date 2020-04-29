using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Commands
{
    public class OrganizationCreateCommand : Organization
    {
        public StackRankType StackRankType { get; set; }
    }
}
