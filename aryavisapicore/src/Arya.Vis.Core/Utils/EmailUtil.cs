using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Arya.Vis.Core.Utils
{
    public static class EmailUtil
    {

        /// <summary>
        /// Validates if given email is RFC 2822 compliant
        /// </summary>
        /// <param name="email">Email address of which the format to be validated</param>
        /// <returns>true if the format is valid else false</returns>
        public static bool IsValidEmailFormat(string email)
        {
            // Refer https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
            // Refer https://stackoverflow.com/a/28156797/4664244 (To support international characters like chinese)
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([\p{L}0-9]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[\p{L}0-9])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([\p{L}0-9][-\p{L}0-9]*[\p{L}0-9]*\.)+[\p{L}0-9][\-\p{L}0-9]{0,22}[\p{L}0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }

}
