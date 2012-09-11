using System;
using System.Text;
using MattBarton.NETMF.Utilities.Services;
using MattBarton.NETMF.Utilities.Enumerations;

namespace MattBarton.NETMF.Utilities
{
    public class HttpRequest
    {
        #region Fields

        private HttpMethod _method;
        private string _hostname;
        private string _path;
        private int _port = 80;
        private int _numberOfArguments = 0;
        private string[] _argumentNames = new string[] { };
        private string[] _argumentValues = new string[] { };
        private string _body = "";

        #endregion

        #region Properties

        public HttpMethod Method
        {
            get
            {
                return this._method;
            }
        }

        public string Hostname
        {
            get
            {
                return this._hostname;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            } 
        }

        public int Port
        {
            get
            {
                return this._port;
            }
        }

        public int StatusCode
        {
            get;
            private set;
        }

        public bool Success
        {
            get
            {
                if (this.StatusCode.ToString().Substring(0, 1) == "2")
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Constructors

        public HttpRequest(string url, HttpMethod method = HttpMethod.GET, int port = 0)
        {
            this.GetHostnameAndPathFromUrl(url);
            this._method = method;
            this._port = port != 0 ? port : this._port;
        }

        #endregion

        #region Public Methods

        public Byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(this.BuildFullRequest());
        }

        public void AddArgument(string name, string value)
        {
            if (name != "")
            {
                var temp = new string[this._numberOfArguments + 1];
                this._argumentNames.CopyTo(temp, 0);
                temp[this._numberOfArguments] = EncodingService.UrlEncode(name);
                this._argumentNames = temp;

                temp = new string[this._numberOfArguments + 1];
                this._argumentValues.CopyTo(temp, 0);
                temp[this._numberOfArguments] = EncodingService.UrlEncode(value);
                this._argumentValues = temp;

                this._numberOfArguments++;
            }
        }

        public void SetStatusCode(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        #endregion

        #region Private Methods

        private void GetHostnameAndPathFromUrl(string url)
        {
            // TODO: utilise Regex when NETMF 4.2 available
            var pos = url.IndexOf('/');

            if (pos < 0)
            {
                this._hostname = url;
                this._path = "/";
            }
            else
            {
                this._hostname = url.Substring(0, pos);
                this._path = url.Substring(pos);
            }
        }

        private string BuildFullRequest()
        {
            if (this._method == HttpMethod.POST)
            {
                this.BuildPostSpecificDetails();
            }

            var fullRequest = this.BuildInitialLine();
            fullRequest += this.BuildHeaders();
            fullRequest += "\n\n";

            if (this._method == HttpMethod.POST)
            {
                fullRequest += this._body;
            }
            return fullRequest;
        }

        private void BuildPostSpecificDetails()
        {
            this._body = this.ArgumentsToString();
        }

        private string BuildHeaders()
        {
            var headers = "Host: " + this._hostname;
            headers += "\nConnection: Close";

            if (this._method == HttpMethod.POST)
            {
                headers += "\nContent-Type: application/x-www-form-urlencoded";
                headers += "\nContent-Length: " + this._body.Length;
            }

            return headers;
        }

        private string BuildInitialLine()
        {
            var method = "";
            switch (this._method)
            {
                case HttpMethod.POST:
                    method = "POST";
                    break;

                case HttpMethod.GET:
                    method = "GET";
                    break;
            }

            var initialLine = method + " " + this._path;

            if (this.Method == HttpMethod.GET)
            {
                var arguments = this.ArgumentsToString();
                initialLine += arguments.Length > 0 ? "?" + arguments : "";
            }

            initialLine += " HTTP/1.0\n";
            return initialLine;
        }

        private string ArgumentsToString()
        {
            var output = "";
            for (var x = 0; x < this._argumentNames.Length; x++)
            {
                if (x > 0)
                {
                    output += "&";
                }

                output += this._argumentNames[x] + "=" + this._argumentValues[x];
            }
            return output;
        }
        #endregion
    }
}
