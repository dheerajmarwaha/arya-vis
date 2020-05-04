using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arya.Vis.Core.Exceptions
{
    public class CountryNotSupportedException : NotSupportedException
    {
        public Iso2CountryCode UnsupportedCountry { get; set; }
        public IEnumerable<Country> SupportedCountries { get; private set; }
        public CountryNotSupportedException() { }

        public CountryNotSupportedException(Iso2CountryCode unsupportedCountry, IEnumerable<Country> supportedCountries) : base(GetMessage(unsupportedCountry, supportedCountries))
        {
            UnsupportedCountry = unsupportedCountry;
            SupportedCountries = supportedCountries;
        }

        private static string GetMessage(Iso2CountryCode unsupportedCountry, IEnumerable<Country> supportedCountries)
        {
            var supportedCountryCodes = supportedCountries.Select(c => new Iso2CountryCode(c.Iso2Code).ToString());
            var supportedCountryCodesStr = string.Join(",", supportedCountryCodes);
            return $"Requested Country: {unsupportedCountry.ToString()} is not supported! Supported countries are {supportedCountryCodesStr}.";
        }

        public CountryNotSupportedException(string message, Exception ex) : base(message, ex) { }
    }
}
