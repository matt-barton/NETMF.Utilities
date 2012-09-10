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
		/// Perform an http request
		/// </summary>
		/// <param name="request">Represents the details of the request</param>
		/// <returns>Webserver response</returns>
		string Send (HttpRequest request);

		#endregion
	}
}
