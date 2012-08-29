using System;
using System.Net;

namespace MattBarton.NETMF.Utilities.Adapters
{
    /// <summary>
    /// Interface for an adpater class for System.Net.Dns
    /// </summary>
    public interface IDns
    {
        /// <summary>
        /// Get the IPHostEntry for a given host
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        IPHostEntry GetHostEntry(string host);
    }
}
