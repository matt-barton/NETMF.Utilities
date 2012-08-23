using System;
using MattBarton.NETMF.Utilities.Interfaces;
using MattBarton.NETMF.Utilities.Builders;

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
        /// TODO: Make port number non-manadatory - should default to 80
		public string Get(string url, int port, string arguments = "")
		{
            var request = new HttpRequestBuilder()
                .SetUrl(url)
                .SetPort(port)
                .Build();
                
            // TODO: think about refactoring Socket.Connect 
            // to be a part of Socket.Request
			this.Socket.Connect(request.Hostname, request.Port);
            return this.Socket.Request(request);
		}

		/// <summary>
		/// Perform a POST request
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public string Post (string url)
		{
            // TODO: Implement HTTP POST
			throw new NotImplementedException();
		}

		#endregion
	}

}
