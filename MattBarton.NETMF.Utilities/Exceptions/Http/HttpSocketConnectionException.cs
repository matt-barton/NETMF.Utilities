using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpSocketConnectionException : Exception
    {
        #region Constructors

        public HttpSocketConnectionException(string message)
            : base(message)
        {
        }

        public HttpSocketConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
