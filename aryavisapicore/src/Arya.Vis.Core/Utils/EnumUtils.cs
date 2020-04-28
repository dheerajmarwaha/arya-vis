using Arya.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Arya.Vis.Core.Utils
{
    public static class EnumUtils
    {
        // Can't restrict TEnum to System.Enum in C# 6.0. Only available from C# 7.0
        // Hence restricting it to Interfaces which System.Enum implement

        /// <summary>
        /// Converts string type to Enum type
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumString"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidArgumentException">Thrown if value is not in the enumeration list with a user understandable exception message</exception>
        public static TEnum GetEnum<TEnum>(string enumString, string fieldName = null) where TEnum : struct, IConvertible, IFormattable
        {
            if (string.IsNullOrWhiteSpace(enumString))
            {
                throw new ArgumentNullException();
            }

            TEnum realEnum;
            var isValidEnum = Enum.TryParse(enumString, true, out realEnum);
            if (!isValidEnum)
            {
                // Generating comma separated values list to embed in error message
                var validValues = string.Join(",", Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(x => $"'{x.ToString(CultureInfo.InvariantCulture)}'")
                .ToArray());

                var message =
                $"'{enumString}' is not a valid parameter value for '{fieldName}' field. It should be one among {validValues}";
                throw new InvalidArgumentException(nameof(enumString), enumString, message);
            }
            return realEnum;
        }

        /// <summary>
        /// Converts enumerable collection of strings to their respective enums.
        /// </summary>
        /// <typeparam name="TEnum">Type of Enum which the string has to be converted to</typeparam>
        /// <param name="enumStrings">Enumeration of strings to be converted to given enum type</param>
        /// <param name="fieldName">FieldName to be used in error message</param>
        /// <param name="ignoreErrors">Ignore if some elements in the enumeration are invalid enums. Returns only the valid enums enumerations</param>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetEnums<TEnum>(IEnumerable<string> enumStrings, string fieldName, bool ignoreErrors = false) where TEnum : struct, IConvertible, IFormattable
        {
            if (enumStrings == null)
            {
                throw new ArgumentNullException();
            }

            var enums = new List<TEnum>();
            foreach (var enumString in enumStrings)
            {
                try
                {
                    enums.Add(GetEnum<TEnum>(enumString, fieldName));
                }
                catch (InvalidArgumentException)
                {
                    if (!ignoreErrors)
                    {
                        throw;
                    }
                }
            }
            return enums;
        }
    }

}
