using System;
using MattBarton.NETMF.Utilities.Enumerations;

namespace MattBarton.NETMF.Utilities.Builders
{
    public class HttpRequestBuilder
    {
        #region Fields

        private HttpMethod _method = HttpMethod.GET;
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

        public HttpRequestBuilder SetMethod(HttpMethod method)
        {
            this._method = method;
            return this;
        }

        #endregion
    }
}
