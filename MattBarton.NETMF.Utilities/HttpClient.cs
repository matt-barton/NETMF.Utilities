using System;
using MattBarton.NETMF.Utilities.Interfaces;

namespace MattBarton.NETMF.Utilities
{
	public class HttpClient : IHttpClient
	{
		private IHttpSocket _socket;

		/// <summary>
		/// Proxy for an HttpSocket object
		/// </summary>
		private IHttpSocket Socket
		{
			get
			{
				return this._socket ??
				       (this._socket = new HttpSocket());
			}
		}

		#region Constructors
		/// <summary>
		/// Initialise a new instance
		/// </summary>
		public HttpClient ()
		{
		}

		/// <summary>
		/// Initialise a new instance, injecting dependencies
		/// </summary>
		/// <param name="socket"></param>
		public HttpClient (IHttpSocket socket)
		{
			this._socket = socket;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Perform a GET request
		/// </summary>
		/// <param name="url"></param>
		/// <param name="port"> </param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public string Get(string url, int port, string arguments = "")
		{
			this.Socket.Connect(url, port);
			return "";
		}

		/// <summary>
		/// Perform a POST request
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public string Post (string url)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

}
