using System;
using MattBarton.NETMF.Utilities.Interfaces;
using MattBarton.NETMF.Utilities.Builders;
using System.IO;
using System.Text;
using MattBarton.NETMF.Utilities.Exceptions.Http;
using MattBarton.NETMF.Utilities.Data;

namespace MattBarton.NETMF.Utilities
{
	public class HttpClient : IHttpClient
	{
		private IHttpSocket _socket;

        #region Properties

        /// <summary>
		/// Proxy for an HttpSocket object
		/// </summary>
		private IHttpSocket Socket
		{
			get
			{
				return this._socket ??
				       (this._socket = new HttpSocket());
			}
		}

        #endregion

        #region Constructors
        /// <summary>
		/// Initialise a new instance
		/// </summary>
		public HttpClient ()
		{
		}

		/// <summary>
		/// Initialise a new instance, injecting dependencies
		/// </summary>
		/// <param name="socket"></param>
		public HttpClient (IHttpSocket socket)
		{
			this._socket = socket;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Perform a GET request
		/// </summary>
		/// <param name="url"></param>
		/// <param name="port"> </param>
		/// <param name="arguments"></param>
		/// <returns></returns>
        public string Send(HttpRequest request)
        {
            var response = this.Socket.Request(request);
            var content = ExtractContent(response);
            return content;
        }

		#endregion

        #region Private Methods 

        private string ExtractContent(string httpResponse)
        {
            var httpResponseComponents = ExtractComponents(httpResponse);
            CheckStatus(httpResponseComponents.StatusCode);
            return httpResponseComponents.Content;
        }

        private static void CheckStatus(int statusCode)
        {
            var statusCategory = statusCode.ToString().ToCharArray()[0];

            if (statusCategory == '2')
            {
                // success
            }
            else
            {
                switch (statusCategory)
                {
                    case '1':
                        throw new HttpUnhandledStatusException(
                            "The server responded with the continuation status code "
                            + statusCode + " which I cannot deal with");
                        break;

                    case '3':
                        throw new HttpUnhandledRedirectionException(
                            "The server responded with the redirectionstatus code "
                            + statusCode + " which I cannot deal with");
                        break;

                    case '4':
                        throw new HttpClientErrorException(
                            "The server responded with the client error code "
                            + statusCode + ".  You did something wrong.");
                        break;

                    case '5':
                        throw new HttpServerErrorException(
                            "The server responded with the error code "
                            + statusCode + ".  Something went wrong which you have no control over.");
                        break;

                    default:
                        throw new HttpInvalidResponseException(
                            "The server responded with the error code "
                            + statusCode + " which I dont understand");
                        break;
                }
            }
        }

        private static HttpResponseComponents ExtractComponents(string httpResponse)
        {
            var byteArray = Encoding.UTF8.GetBytes(httpResponse);
            var stream = new MemoryStream(byteArray);
            var reader = new StreamReader(stream);

            string statusLine = "";
            string response = "";

            var headers = new string[] { };
            bool headersFound = false;

            var line = reader.ReadLine();
            while (line != null)
            {
                if (line == "")
                {
                    /* StreamReader.ReadLine appears to be buggy.  
                     * It should return null at the end of the stream
                     * but is returning empty string.
                     * Need to test for end of stream here */
                    if (reader.EndOfStream)
                    {
                        break;
                    }
                    
                    if (statusLine == "")
                    {
                        throw new HttpInvalidResponseException("No HTTP Status line found in response");
                    }
                    headersFound = true;
                }
                else
                {
                    if (statusLine == "")
                    {
                        statusLine = line;
                    }
                    else
                    {
                        if (headersFound)
                        {
                            response += line;
                        }
                        else
                        {
                            var temp = new string[headers.Length + 1];
                            headers.CopyTo(temp, 0);
                            temp[headers.Length] = line;
                            headers = temp;
                        }
                    }
                }
                line = reader.ReadLine();
            }

            var statusComponents = statusLine.Split(' ');

            if (statusComponents.Length < 3)
            {
                throw new HttpInvalidResponseException("Invalid status line \"" + statusLine + "\"");
            }
            int statusCode = Int32.Parse(statusComponents[1]);

            return new HttpResponseComponents
            {
                Headers = headers,
                StatusCode = statusCode,
                Content = response
            };
        }

        #endregion

    }

}
