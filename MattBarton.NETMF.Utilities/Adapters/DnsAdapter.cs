using System;
using System.Net;

namespace MattBarton.NETMF.Utilities.Adapters
{
    /// <summary>
    /// Adapter class for System.Net.Dns
    /// Allows unit-test mocking of the static Dns class via the adapter
    /// </summary>
    public class DnsAdapter : IDns
    {
        public IPHostEntry GetHostEntry(string host)
        {
            return Dns.GetHostEntry(host);
        }
    }
}
