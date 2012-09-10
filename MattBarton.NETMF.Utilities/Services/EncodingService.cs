using System;
using System.Text;

namespace MattBarton.NETMF.Utilities.Services
{
    public static class EncodingService
    {
        public static string UrlEncode(string str)
        {
            var chars = str.ToCharArray();
            var length = str.Length;
            var sourcePointer = 0;
            var output = "";

            while (true)
            {
                if (sourcePointer == length)
                {
                    return output;
                }

                char c = chars[sourcePointer];

                if ((' ' <= c && c <= '/') || (':' <= c && c <= '@'))
                {
                    output += "%" + ((int)c).ToString("X");

                }
                else if (('0' <= c && c <= '9') || ('A' <= c && c <= '~'))
                {
                    output += c.ToString();
                }

                sourcePointer++;
            }
        }

        public static string UrlDecode(string str)
        {
            return "";
        }
    }
}
