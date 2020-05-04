using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Arya.Vis.Container.Web.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AppCode
    {
        [EnumMember(Value = "UNKNOWN_ERROR")]
        UnknownError, [EnumMember(Value = "INVALID_INPUT_PARAMETER_VALUE")]
        InvalidInputParameterValue, [EnumMember(Value = "UNSUPPORTED_COUNTRY")]
        UnsupportedCountry, [EnumMember(Value = "UNAUTHORIZED_ENTITY_ACCESS")]
        UnauthorizedEntityAccess, [EnumMember(Value = "UNAUTHORIZED_OPERATION_EXCEPTION")]
        UnauthorizedOperationException, [EnumMember(Value = "UNSUPPORTED_FILE_EXTENSION")]
        UnsupportedFileExtension, [EnumMember(Value = "UNSUPPORTED_TAG_TYPE")]
        UnsupportedTagType, [EnumMember(Value = "UNSUPPORTED_FEATURE")]
        UnsupportedFeature
    }
}
