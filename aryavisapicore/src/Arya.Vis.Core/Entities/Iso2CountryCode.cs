using Arya.Exceptions;
using Arya.Vis.Core.JsonConverters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Arya.Vis.Core.Entities
{
    [JsonConverter(typeof(Iso2CountryCodeConverter))]
    public class Iso2CountryCode : IComparable, IComparable<Iso2CountryCode>, IEquatable<Iso2CountryCode>, IFormattable
    {
        private readonly string _code;

        public Iso2CountryCode(string countryCode)
        {
            ValidateCountryCode(countryCode);
            _code = countryCode.ToUpper();
        }

        private void ValidateCountryCode(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            {
                throw new InvalidArgumentException(nameof(countryCode), countryCode, "non empty or non null string of length 2");
            }
            // TODO: Add CountryCode ENUM and validate against it
        }

        public static Iso2CountryCode Parse(string countryCode)
        {
            return new Iso2CountryCode(countryCode);
        }

        public int CompareTo(Iso2CountryCode other)
        {
            return _code.CompareTo(other._code);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Iso2CountryCode;
            return _code.CompareTo(other._code);
        }

        public bool Equals(Iso2CountryCode other)
        {
            return _code.Equals(other._code);
        }

        public override string ToString()
        {
            return _code;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _code.ToString(formatProvider);
        }

        public static implicit operator string(Iso2CountryCode c)
        {
            return c?._code;
        }

        public static implicit operator Iso2CountryCode(string s)
        {
            return new Iso2CountryCode(s);
        }
    }

}
