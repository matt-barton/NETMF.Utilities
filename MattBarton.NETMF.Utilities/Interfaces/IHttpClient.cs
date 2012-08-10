using System;

namespace MattBarton.NETMF.Utilities.Interfaces
{
	/// <summary>
	/// Facade for an http client
	/// </summary>
	public interface IHttpClient
	{
		#region Method Declarations

		/// <summary>
		/// Perform an http GET
		/// </summary>
		/// <param name="url">URL for the request</param>
		/// <param name="arguments">Http argument string</param>
		/// <returns>Webserver response</returns>
		string Get (string url, int port, string arguments = "");

		/// <summary>
		/// Perform an http POST
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		string Post (string url);

		#endregion
	}
}
