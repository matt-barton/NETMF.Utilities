using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpRequestException : Exception
    {
        #region Properties

        public HttpRequest Request
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public HttpRequestException(string message)
            : base(message)
        { 
        }

        public HttpRequestException(string message, HttpRequest request)
            : base(message)
        {
            this.Request = request;
        }

        public HttpRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
