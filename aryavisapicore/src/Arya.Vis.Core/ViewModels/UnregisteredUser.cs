using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;

namespace Arya.Vis.Core.ViewModels
{
    public class UnregisteredUser
    {
        public Guid? UserGuid { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public Iso2CountryCode CountryCode { get; set; }
        public IList<string> Industries { get; set; }
        public ProfileType? ProfileType { get; set; }
    }
    public enum ProfileType {
        Pulse,
        AryaVis
    }
}