using Arya.ServiceBus;
using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Core.Events
{
    public class OrganizationCreatedEvent : IEvent
    {
        public Organization Organization { get; set; }
    }
}
