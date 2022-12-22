using System.Security.Cryptography;
using System.Text;

#pragma warning disable 1591

namespace EltraCommon.Helpers
{
    public static class CryptHelpers
    {
        public static string ToSha256(string text)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                using (var sha256Hash = SHA256.Create())
                {
                    var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
                    var builder = new StringBuilder();

                    foreach (var b in bytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }

                    result = builder.ToString();
                }
            }

            return result;
        }

        public static string ToMD5(string text)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                var inputBytes = Encoding.ASCII.GetBytes(text);

                result = ToMD5(inputBytes);
            }

            return result;
        }

        public static string ToMD5(byte[] inputBytes)
        {
            string result = string.Empty;

            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                result = sb.ToString();
            }

            return result;
        }
    }
}
