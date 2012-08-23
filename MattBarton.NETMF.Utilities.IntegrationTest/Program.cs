using System;
using Microsoft.SPOT;

namespace MattBarton.NETMF.Utilities.IntegrationTest
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print(
                new HttpClient().Get("www.google.co.uk", 80));
        }

    }
}
