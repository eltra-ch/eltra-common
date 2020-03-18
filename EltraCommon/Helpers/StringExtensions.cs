using System.Text.RegularExpressions;

namespace EltraCommon.Helpers
{
    public static class StringExtensions
    {
        public static bool IsPhoneNumber(this string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
    }
}
