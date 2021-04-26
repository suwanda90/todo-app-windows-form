using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ApplicationCore.Helpers
{
    public static class SecurityHelper
    {
        private static readonly string Key = "628127SKAsndhowqye1ADNKNQ:MFIPQWU!@*!&$!)$(!)@SHAJS@&#^*@NHDNAL>SMAOIFHNAOJD";
        public static bool IsGuid(this string candidate)
        {
            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
            bool isValid = false;

            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public static bool IsNumber(this string candidate)
        {
            bool isValid = false;

            if (candidate != null)
            {
                isValid = candidate.All(char.IsDigit);
            }

            return isValid;
        }

        public static bool IsIntegralTypes(this string candidate, Type type)
        {
            bool isValid = false;

            if (candidate != null)
            {
                if (type == typeof(sbyte) || type == typeof(sbyte?))
                {
                    isValid = sbyte.TryParse(candidate, out _);
                }
                else if (type == typeof(byte) || type == typeof(byte?))
                {
                    isValid = byte.TryParse(candidate, out _);
                }
                else if (type == typeof(short) || type == typeof(short?))
                {
                    isValid = short.TryParse(candidate, out _);
                }
                else if (type == typeof(ushort) || type == typeof(ushort?))
                {
                    isValid = ushort.TryParse(candidate, out _);
                }
                else if (type == typeof(int) || type == typeof(int?))
                {
                    isValid = int.TryParse(candidate, out _);
                }
                else if (type == typeof(long) || type == typeof(long?))
                {
                    isValid = long.TryParse(candidate, out _);
                }
                else if (type == typeof(ulong) || type == typeof(ulong?))
                {
                    isValid = ulong.TryParse(candidate, out _);
                }
            }

            return isValid;
        }

        public static bool IsDate(this string candidate)
        {
            bool isValid = false;

            if (candidate != null)
            {
                string format = "yyyy-MM-dd";
                isValid = DateTime.TryParseExact(candidate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
            }

            return isValid;
        }

        public static bool IsBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String) ||
                base64String.Length % 4 != 0 ||
                base64String.Contains(" ") ||
                base64String.Contains("\t") ||
                base64String.Contains("\r") ||
                base64String.Contains("\n"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string ToBase64Encode(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] textBytes = Encoding.UTF8.GetBytes(text + Key);
            return Convert.ToBase64String(textBytes);
        }

        public static string ToBase64Decode(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] base64EncodedBytes = Convert.FromBase64String(text);
            var textDecode = Encoding.UTF8.GetString(base64EncodedBytes);
            int length = textDecode.Length - Key.Length;

            return textDecode.Substring(0, length);
        }

        public static string ToBase64EncodeWithKey(this string text, string key)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(key))
            {
                return text;
            }

            var keyValue = key.ToBase64Decode();

            byte[] textBytes = Encoding.UTF8.GetBytes(text + keyValue);
            return Convert.ToBase64String(textBytes);
        }

        public static string ToBase64DecodeWithKey(this string text, string key)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(key))
            {
                return text;
            }

            var keyValue = key.ToBase64Decode();

            byte[] base64EncodedBytes = Convert.FromBase64String(text);
            var textDecode = Encoding.UTF8.GetString(base64EncodedBytes);
            int length = textDecode.Length - keyValue.Length;

            return textDecode.Substring(0, length);
        }
    }
}
