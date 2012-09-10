using System;

namespace MattBarton.NETMF.Utilities.Builders
{
    public class HttpRequestBuilder
    {
        #region Fields

        private string _method;
        private string _url;
        private int _port = 0;

        #endregion

        #region Public Methods

        public HttpRequest Build()
        {
            return new HttpRequest(this._url, this._method, this._port);
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

        #endregion
    }
}
