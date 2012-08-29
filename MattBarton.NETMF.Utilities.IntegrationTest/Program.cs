using System;
using Microsoft.SPOT;

namespace MattBarton.NETMF.Utilities.IntegrationTest
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print(
                new HttpClient().Get("mattbarton.org/netmf_utilities_http_get_test.txt"));
        }

    }
}
