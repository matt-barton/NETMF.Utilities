using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpTimeoutException : Exception
    {
        #region Constructors

        public HttpTimeoutException(string message)
            : base(message)
        { 
        }

        public HttpTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
