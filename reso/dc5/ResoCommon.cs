using MediaBrowser.Model.Globalization;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ResoWebApi.Common
{
    public static class ResoCommon
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value?.Trim());
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value?.Trim());
        }

        public static DateTime GetDateTimeFromTimeStamp(this string value)
        {
            var result = DateTime.MinValue;
            if (value.IsNotNullOrEmpty())
            {
                var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                result = dtDateTime.AddSeconds(Convert.ToInt64(value)).ToUniversalTime();
            }
            return result;
        }

        public static DateTime GetDateTime(this string value)
        {
            DateTime.TryParse(value, out DateTime startDateTime);
            return startDateTime;
        }

        public static string Description(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return description
            return attributes.Any() ? ((DescriptionAttribute)attributes.ElementAt(0)).Description : "Description Not Found";
        }

        public static TEnum GetEnumFromValue<TEnum>(this string value) where TEnum : Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static TNumericType GetDigitFromString<TNumericType>(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                value = "0";
            }
            var digit = Regex.Match(value, @"\d+").Value;
            return (TNumericType)Convert.ChangeType(digit, typeof(TNumericType));
        }

        public static string GetLettersFromString(this string value)
        {
            var result = value;
            if (value.IsNotNullOrEmpty())
            {
                result = new string(value.Where(char.IsLetter).ToArray());
            }

            return result;
        }

        public static bool IsEqual(this string leftSidevalue, string rightSideValue)
        {
            if (leftSidevalue.IsNotNullOrEmpty())
            {
                leftSidevalue = Regex.Replace(leftSidevalue.ToLower(), @"\s", "");
            }

            if (rightSideValue.IsNotNullOrEmpty())
            {
                rightSideValue = Regex.Replace(rightSideValue.ToLower(), @"\s", "");
            }

            return leftSidevalue == rightSideValue;
        }

        public static string GetCountryISOCode(this string countryName)
        {
            string result = String.Empty;
            if (countryName.IsNotNullOrEmpty())
            {
                CountryList countries = CountryList.CreateList(true, false);
                CountryInfo country = countries.Find(countryName);
                result = country?.TwoLetterISORegionName;
            }
            return result;
        }

        public static async Task<string> GetRequestBody(this Stream body)
        {
            using var reader = new StreamReader(body);
            return await reader.ReadToEndAsync();
        }
    }
}
