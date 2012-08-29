using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpUnknownHostException : Exception
    {
        #region Constructors

        public HttpUnknownHostException(string message)
            : base(message)
        { 
        }

        public HttpUnknownHostException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
