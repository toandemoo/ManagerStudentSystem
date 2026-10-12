using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ManagerStudentSystem.Services.PaymentProvider
{
    public class Utils
    {
        private static char GetHexUpper(int value)
        {
            return value switch
            {
                >= 0 and <= 9 => (char)('0' + value),
                >= 10 and <= 15 => (char)('A' + value - 10),
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private static bool IsUrlSafeChar(char c)
        {
            return
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                (c >= '0' && c <= '9') ||
                c == '-' ||
                c == '_' ||
                c == '.' ||
                c == '!' ||
                c == '~' ||
                c == '*' ||
                c == '\'' ||
                c == '(' ||
                c == ')';
        }

        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            var result = new StringBuilder(bytes.Length);

            foreach (byte b in bytes)
            {
                char c = (char)b;

                if (IsUrlSafeChar(c))
                {
                    result.Append(c);
                }
                else if (b == (byte)' ')
                {
                    result.Append('+');
                }
                else
                {
                    result.Append('%');
                    result.Append(GetHexUpper(b >> 4));
                    result.Append(GetHexUpper(b & 0x0F));
                }
            }
            return result.ToString();
        }
    }
}