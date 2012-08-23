using System;
using System.Net;
using System.Net.Sockets;

namespace MattBarton.NETMF.Utilities.Adapters
{
    /// <summary>
    /// Interface for an adpater class for System.Net.Sockets.Socket
    /// </summary>
    public interface ISocket
    {
        void Connect(EndPoint endPoint);

        int Send(Byte[] bytes);

        Byte[] Receive(int bufferSize);

        bool Poll(int microseconds, SelectMode mode);

        int Available();
    }
}
