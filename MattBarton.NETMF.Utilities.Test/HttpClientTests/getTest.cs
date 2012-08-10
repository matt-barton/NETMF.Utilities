using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MattBarton.NETMF.Utilities.Interfaces;
using Moq;
using NUnit.Framework;

namespace MattBarton.NETMF.Utilities.Test.HttpClientTests
{
	/// <summary>
	/// Tests for the Get method of the HttpClient
	/// </summary>
	[TestFixture]
	class GetTest
	{
		[Test]
		public void Given_url_and_port_are_specified_When_get_Then_socket_is_connected_for_url_and_port()
		{
			// setup
			var mockSocket = new Mock<IHttpSocket>();
			var target = new HttpClient(mockSocket.Object);
			var mockUrl = "http://www.test.com";
			var mockPort = 80;

			// execution
			target.Get(mockUrl, mockPort);

			// assert
			mockSocket.Verify(
				s => s.Connect(
					It.Is<string>(p => p == mockUrl),
					It.Is<int>(p => p == mockPort)));
		}


	}
}
