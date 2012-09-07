using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MattBarton.UnitTesting;

namespace MattBarton.NETMF.Utilities.Test.Builders
{
    class HttpResponseBuilder : IBuilder<string>
    {
        #region Fields

        private int _statusCode;
        private string _statusDescriptor = ".";
        private List<string> _headers = new List<string>();
        private string _response;

        #endregion

        #region Public Methods

        public string Build()
        {
            var response = new StringBuilder()
                .Append("HTTP/1.0 ")
                .Append(this._statusCode)
                .Append(" ")
                .Append(this._statusDescriptor)
                .Append("\n");

            foreach (var header in this._headers)
            {
                response.AppendLine(header);
            }

            response.Append("\n");
            response.Append(this._response);

            return response.ToString();
        }

        public HttpResponseBuilder SetStatusCode(int statusCode)
        {
            this._statusCode = statusCode;
            return this;
        }

        public HttpResponseBuilder SetStatusDescriptor(string statusDescriptor)
        {
            this._statusDescriptor = statusDescriptor;
            return this;
        }

        public HttpResponseBuilder SetResponse(string response)
        {
            this._response = response;
            return this;
        }

        public HttpResponseBuilder SetHeaders(List<string> headers)
        {
            this._headers = headers;
            return this;
        }

        public HttpResponseBuilder AddHeader(string header)
        {
            this._headers.Add(header);
            return this;
        }

        #endregion
    }
}
