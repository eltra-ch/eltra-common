using EltraCommon.Logger;
using System;
using System.Text;

#pragma warning disable 1591

namespace EltraCommon.Extensions
{
    public static class StringExtensions
    {
        public static string ToBase64(this string text)
        {
            string result = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(text);

                    result = Convert.ToBase64String(bytes);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"StringExtensions - ToBase64", e);
            }

            return result;
        }

        public static string FromBase64(this string base64)
        {
            string result = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(base64))
                {
                    var bytes = Convert.FromBase64String(base64);

                    result = Encoding.UTF8.GetString(bytes);
                }
            }
            catch(Exception e)
            {
                MsgLogger.Exception($"StringExtensions - FromBase64", e);
            }

            return result;
        }

        public static bool IsPhoneNumber(this string phoneNumber)
        {
            const int RegionalPhoneLength = 9;
            const int InternationalPhoneLength = 11;

            string normalized = NormalizePhoneNumber(phoneNumber);

            return normalized.Length == RegionalPhoneLength || normalized.Length == InternationalPhoneLength;
        }

        private static string NormalizePhoneNumber(string phoneNumber)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = phoneNumber.Replace(" ", string.Empty);

                if (phoneNumber.StartsWith("+"))
                {
                    phoneNumber = phoneNumber.Insert(0, "00");
                }

                var entities = phoneNumber.Split(new[] { ' ', '-', '+', '(', ')', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                string normalizedPhoneNum = string.Empty;

                foreach (var entity in entities)
                {
                    foreach (var c in entity)
                    {
                        if (char.IsDigit(c))
                        {
                            normalizedPhoneNum += c;
                        }
                    }
                }

                if (normalizedPhoneNum.StartsWith("0"))
                {
                    int firstNotZero = 0;
                    foreach (var c in normalizedPhoneNum)
                    {
                        if (c != '0')
                        {
                            break;
                        }

                        firstNotZero++;
                    }

                    result = normalizedPhoneNum.Substring(firstNotZero);
                }
            }

            return result;
        }

    }
}
