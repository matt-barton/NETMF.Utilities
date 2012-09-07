using System;
using System.Text;

namespace MattBarton.NETMF.Utilities
{
    public class HttpRequest
    {
        #region Fields

        private string _method;
        private string _hostname;
        private string _path;
        private int _port;
        private string _request;

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

        public string Request
        {
            get
            {
                return this._request;
            }
        }

        #endregion

        #region Constructors

        public HttpRequest(string method, string url, int port, string request)
        {
            this.GetHostnameAndUriFromUrl(url);
            this._port = port;
            this._request = request;
            this._method = method;
        }

        #endregion

        #region Public Methods

        public Byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(this.BuildFullRequest());
        }

        #endregion

        #region Private Methods

        private void GetHostnameAndUriFromUrl(string url)
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
            var fullRequest = this._method + " " + this._path + " HTTP/1.0\n";
            fullRequest += "Host: " + this._hostname + "\n\n";
            fullRequest += this._request;
            return fullRequest;
        }

        #endregion
    }
}
