using System;
using Microsoft.SPOT;
using MattBarton.NETMF.Utilities.Builders;
using MattBarton.NETMF.Utilities.Enumerations;

namespace MattBarton.NETMF.Utilities.IntegrationTest
{
    public class Program
    {
        public static void Main()
        {
            string response;

            Debug.Print("* -------------------------------");
            Debug.Print("* IntegrationTest");
            Debug.Print("* -------------------------------");
            Debug.Print("");

            string getUrl = "mattbarton.org/netmf_utilities_http_get_test.php";
            HttpRequest getRequest = new HttpRequestBuilder()
                .SetUrl(getUrl)
                .Build();

            getRequest.AddArgument("arg1", "value1");
            getRequest.AddArgument("color", "yellow");

            Debug.Print("Getting " + getUrl);
            Debug.Print("");
            try
            {
                response = new HttpClient().Send(getRequest);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            Debug.Print(response);
            Debug.Print("");


            string postUrl = "mattbarton.org/netmf_utilities_http_post_test.php";
            HttpRequest postRequest = new HttpRequestBuilder()
                .SetUrl(postUrl)
                .SetMethod(HttpMethod.POST)
                .Build();

            postRequest.AddArgument("arg1", "value1");
            postRequest.AddArgument("color", "yellow");

            Debug.Print("Posting to " + postUrl);
            Debug.Print("");
            try
            {
                response = new HttpClient().Send(postRequest);
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
