using System;
using Microsoft.SPOT;
using MattBarton.NETMF.Utilities.Builders;

namespace MattBarton.NETMF.Utilities.IntegrationTest
{
    public class Program
    {
        public static void Main()
        {
            string response;
            string url = "mattbarton.org/netmf_utilities_http_get_test.txt";
            HttpRequest request = new HttpRequestBuilder()
                .SetUrl(url)
                .Build();

            Debug.Print("* -------------------------------");
            Debug.Print("* IntegrationTest");
            Debug.Print("* -------------------------------");
            Debug.Print("");
            Debug.Print("Getting " + url);
            Debug.Print("");
            try
            {
                response = new HttpClient().Get(request);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            Debug.Print(response);
            Debug.Print("");
            Debug.Print("* -------------------------------");
            Debug.Print("* End");
            Debug.Print("* -------------------------------");
        }

    }
}
