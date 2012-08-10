using System.Net;
using System.Net.Sockets;
using MattBarton.NETMF.Utilities.Interfaces;

namespace MattBarton.NETMF.Utilities
{
	public class HttpSocket : IHttpSocket
	{
		private Socket _socket;
		private string _host;
		private int _port;

		public void Connect (string url, int port)
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(url);
			this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this._socket.Connect(new IPEndPoint(hostEntry.AddressList[0], port));
		}
	}
}
