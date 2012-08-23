using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MattBarton.NETMF.Utilities.Interfaces;
using Moq;
using NUnit.Framework;
using System.Net.Sockets;

namespace MattBarton.NETMF.Utilities.Test.HttpClientTests
{
	/// <summary>
	/// Tests for the Get method of the HttpClient
	/// </summary>
	[TestFixture]
	class GetTest
	{
		[Test]
		public void Given_url_and_port_are_specified_When_get_Then_socket_is_connected_for_hostname_and_port()
		{
			// setup
			var mockSocket = new Mock<IHttpSocket>();
			var target = new HttpClient(mockSocket.Object);
			var mockUrl = "www.test.com";
			var mockPort = 80;

			// execution
			target.Get(mockUrl, mockPort);

			// assert
			mockSocket.Verify(
				s => s.Connect(
					It.Is<string>(p => p == mockUrl),
					It.Is<int>(p => p == mockPort)));
		}

        [Test]
        public void Given_url_is_specified_When_get_Then_socket_is_connected_for_hostname_without_full_path()
        {
            // setup
            var mockSocket = new Mock<IHttpSocket>();
            var target = new HttpClient(mockSocket.Object);
            var mockHostname = "www.test.com";
            var mockPath = mockHostname + "/subdir/resource.html";

            // execution
            target.Get(mockPath, -1);

            // assert
            mockSocket.Verify(
                s => s.Connect(
                    It.Is<string>(p => p == mockHostname),
                    It.IsAny<int>()));
        }

        [Test]
        public void Given_url_and_port_are_set_When_get_Then_request_is_sent_via_socket()
        {
            // setup
            var mockHostname = "www.test.com";
            var mockPath = "/resource.html";
            var mockUrl = mockHostname + mockPath;
            var mockPort = 8080;
            var mockSocket = new Mock<IHttpSocket>();

            var target = new HttpClient(mockSocket.Object);

            // execution
            target.Get(mockUrl, mockPort);

            // assertion
            mockSocket.Verify(
                s => s.Request(It.Is<HttpRequest>(
                    hr => hr.Hostname == mockHostname &&
                        hr.Path == mockPath &&
                        hr.Port == mockPort)),
                Times.Once(),
                "Request not sent via socket");
        }

        [Test]
        public void Given_that_socket_returns_a_response_When_get_Then_response_returned()
        {
            // setup
            var mockResponse = "Socket response";
            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution
            var result = target.Get("", -1);

            // assertion
            Assert.AreEqual(mockResponse, result, "Returned response is not correct");
        }
	}
}
