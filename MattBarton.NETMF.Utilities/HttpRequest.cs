using System;
using System.Text;
using MattBarton.NETMF.Utilities.Services;

namespace MattBarton.NETMF.Utilities
{
    public class HttpRequest
    {
        #region Fields

        private string _method = "GET";
        private string _hostname;
        private string _path;
        private int _port = 80;
        private int _numberOfArguments = 0;
        private string[] _argumentNames = new string[] { };
        private string[] _argumentValues = new string[] { };

        #endregion

        #region Properties

        public string Method
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

        #endregion

        #region Constructors

        public HttpRequest(string url, string method = null, int port = 0)
        {
            this.GetHostnameAndPathFromUrl(url);
            this._method = method != null ? method : this._method;
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
            var fullRequest = this.BuildInitialLine();
            fullRequest += this.BuildStandardHeaders();
            fullRequest += "\n";
            return fullRequest;
        }

        private string BuildStandardHeaders()
        {
            return "Host: " + this._hostname + "\n"
                + "Connection: Close\n";
        }
        private string BuildInitialLine()
        {
            var initialLine = this._method + " " + this._path;
            for (var x = 0; x < this._argumentNames.Length; x++)
            {
                if (x == 0)
                {
                    initialLine += "?";
                }
                else
                {
                    initialLine += "&";
                }

                initialLine += this._argumentNames[x] + "=" + this._argumentValues[x];
            }
            initialLine += " HTTP/1.0\n";
            return initialLine;
        }
        #endregion
    }
}
