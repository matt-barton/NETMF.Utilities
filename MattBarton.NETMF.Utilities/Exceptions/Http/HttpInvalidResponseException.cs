using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpInvalidResponseException : Exception
    {
        #region Constructors

        public HttpInvalidResponseException(string message)
            : base(message)
        { 
        }

        public HttpInvalidResponseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
