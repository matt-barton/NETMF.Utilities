using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpServerErrorException : Exception
    {
        #region Constructors

        public HttpServerErrorException(string message)
            : base(message)
        { 
        }

        public HttpServerErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
