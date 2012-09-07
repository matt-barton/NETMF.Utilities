using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpUnhandledRedirectionException : Exception
    {
        #region Constructors

        public HttpUnhandledRedirectionException(string message)
            : base(message)
        { 
        }

        public HttpUnhandledRedirectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
