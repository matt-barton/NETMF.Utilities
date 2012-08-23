using System;

namespace MattBarton.NETMF.Utilities.Interfaces
{
	public interface IHttpSocket
	{
		void Connect(string url, int port);

        String Request(HttpRequest request);
	}
}
