using Arya.Vis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Arya.Vis.Core.JsonConverters
{
    public class Iso2CountryCodeConverter : JsonConverter<Iso2CountryCode>
    {
        public override Iso2CountryCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }
            return new Iso2CountryCode(s);
        }

        public override void Write(Utf8JsonWriter writer, Iso2CountryCode value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStringValue(value.ToString());
        }
    }

}
