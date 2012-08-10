using System;

namespace MattBarton.NETMF.Utilities.Models
{
	/// <summary>
	/// Models an Http Request
	/// </summary>
	class HttpRequest
	{
		/// <summary>
		///  The host name or ip 
		/// </summary>
		public string Host { get; set; }
		public int Port { get; set; }
		public string RequestType { get; set; }
		public string Request { get; set; }

	}
}
