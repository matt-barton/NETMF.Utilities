using System;

namespace MattBarton.NETMF.Utilities.Builders
{
    public class HttpRequestBuilder
    {
        #region Fields

        private string _method = "GET";
        private string _url;
        private int _port = 80;
        private string _request;

        #endregion

        #region Public Methods

        public HttpRequest Build()
        {
            return new HttpRequest(this._method, this._url, this._port, this._request);
        }

        public HttpRequestBuilder SetUrl(string url)
        {
            this._url = url;
            return this;
        }

        public HttpRequestBuilder SetPort(int port)
        {
            this._port = port;
            return this;
        }

        public HttpRequestBuilder SetMethod(string method)
        {
            this._method = method;
            return this;
        }

        public HttpRequestBuilder SetRequest(string request)
        {
            this._request = request;
            return this;
        }

        #endregion
    }
}
