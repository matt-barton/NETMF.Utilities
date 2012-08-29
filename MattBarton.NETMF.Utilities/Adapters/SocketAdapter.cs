using System;
using System.Net.Sockets;
using System.Net;

namespace MattBarton.NETMF.Utilities.Adapters
{
    /// <summary>
    /// Adapter class for System.Net.Sockets.Socket
    /// Allows unit-test mocking of the Socket object via the adapter
    /// </summary>
    public class SocketAdapter : ISocket
    {
        #region Fields

        private Socket _socket;

        #endregion

        #region Constructor

        public SocketAdapter(AddressFamily addressFamily,
            SocketType socketType, ProtocolType protocolType)
        {
            this._socket = new Socket(addressFamily, socketType, protocolType);
        }

        #endregion

        #region Public Methods

        public void Connect(EndPoint endPoint)
        {
            this._socket.Connect(endPoint);
        }

        public int Send(Byte[] bytes)
        {
            return this._socket.Send(bytes);
        }

        public Byte[] Receive(int bufferSize)
        {
            var bytes = new Byte[bufferSize];
            this._socket.Receive(bytes);
            return bytes;
        }

        public bool Poll (int microSeconds, SelectMode mode)
        {
            return this._socket.Poll(microSeconds, mode);
        }

        public int Available()
        {
            return this._socket.Available;
        }

        #endregion
    }
}
