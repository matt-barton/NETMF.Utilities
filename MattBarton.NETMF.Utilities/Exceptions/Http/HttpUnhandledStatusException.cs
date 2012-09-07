using System;

namespace MattBarton.NETMF.Utilities.Exceptions.Http
{
    [Serializable]
    class HttpUnhandledStatusException : Exception
    {
        #region Constructors

        public HttpUnhandledStatusException(string message)
            : base(message)
        { 
        }

        public HttpUnhandledStatusException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
