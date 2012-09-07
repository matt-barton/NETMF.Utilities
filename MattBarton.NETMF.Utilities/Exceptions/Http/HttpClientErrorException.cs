using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpClientErrorException : Exception
    {
        #region Constructors

        public HttpClientErrorException(string message)
            : base(message)
        { 
        }

        public HttpClientErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
