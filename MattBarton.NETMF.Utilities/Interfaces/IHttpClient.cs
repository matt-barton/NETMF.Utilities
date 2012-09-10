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
		/// <param name="request">Represents the details of the request</param>
		/// <returns>Webserver response</returns>
		string Get (HttpRequest request);

		/// <summary>
		/// Perform an http POST
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		string Post (HttpRequest request);

		#endregion
	}
}
