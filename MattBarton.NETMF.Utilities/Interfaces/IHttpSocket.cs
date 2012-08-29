using System;

namespace MattBarton.NETMF.Utilities.Interfaces
{
	public interface IHttpSocket
	{
        String Request(HttpRequest request);
	}
}
