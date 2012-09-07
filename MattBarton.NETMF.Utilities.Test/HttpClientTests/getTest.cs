using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MattBarton.NETMF.Utilities.Interfaces;
using Moq;
using NUnit.Framework;
using System.Net.Sockets;
using MattBarton.NETMF.Utilities.Test.Builders;
using MattBarton.NETMF.Utilities.Exceptions.Http;

namespace MattBarton.NETMF.Utilities.Test.HttpClientTests
{
	/// <summary>
	/// Tests for the Get method of the HttpClient
	/// </summary>
	[TestFixture]
	class GetTest
	{
        [Test]
        public void Given_url_and_port_are_set_When_get_Then_request_is_sent_via_socket()
        {
            // setup
            var mockHostname = "www.test.com";
            var mockPath = "/resource.html";
            var mockUrl = mockHostname + mockPath;
            var mockPort = 8080;
            var mockSocket = new Mock<IHttpSocket>();
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(200)
                .SetResponse("response")
                .Build();

            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution
            target.Get(mockUrl, "", mockPort);

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
        public void Given_that_http_reponse_is_not_valid_When_getting_Then_HttpInvalidResponseException_thrown()
        {
            var mockResponse = "";

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution/assertion
            TestDelegate testMethod = () => target.Get("");
            Assert.Throws(typeof(HttpInvalidResponseException), testMethod);
        }


        [Test]
        public void Given_that_http_status_code_1xx_is_returned_When_getting_Then_HttpUnhandledStatusException_thrown()
        {
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(100)
                .SetResponse("Some response")
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution/assertion
            TestDelegate testMethod = () => target.Get("");
            Assert.Throws(typeof(HttpUnhandledStatusException), testMethod);
        }

        [Test]
        public void Given_that_http_status_code_3xx_is_returned_When_getting_Then_HttpUnhandledRedirectionException_thrown()
        {
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(301)
                .SetStatusDescriptor("Moved Permanently")
                .SetResponse("Some response")
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution/assertion
            TestDelegate testMethod = () => target.Get("");
            Assert.Throws(typeof(HttpUnhandledRedirectionException), testMethod);
        }

        [Test]
        public void Given_that_http_status_code_4xx_is_returned_When_getting_Then_HttpClientErrorException_thrown()
        {
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(404)
                .SetStatusDescriptor("Not Found")
                .SetResponse("Some response")
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution/assertion
            TestDelegate testMethod = () => target.Get("");
            Assert.Throws(typeof(HttpClientErrorException), testMethod);
        }

        [Test]
        public void Given_that_http_status_code_5xx_is_returned_When_getting_Then_HttpServerErrorException_thrown()
        {
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(500)
                .SetStatusDescriptor("Server Error")
                .SetResponse("Some response")
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution/assertion
            TestDelegate testMethod = () => target.Get("");
            Assert.Throws(typeof(HttpServerErrorException), testMethod);
        }

        [Test]
        public void Given_that_http_status_code_2xx_is_returned_When_getting_Then_response_is_returned()
        {
            var content = "some response content";
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(200)
                .SetStatusDescriptor("OK")
                .SetResponse(content)
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution
            var result = target.Get("");

            // assertion
            Assert.AreEqual(content, result, "The http reponse was not correct");
        }

        [Test]
        public void Given_that_http_status_code_2xx_And_http_headers_are_returned_When_getting_Then_response_is_returned()
        {
            var content = "some response content";
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(200)
                .SetStatusDescriptor("OK")
                .SetResponse(content)
                .AddHeader("Header-one: header stuff")
                .AddHeader("Second-header: more header stuff")
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution
            var result = target.Get("");

            // assertion
            Assert.AreEqual(content, result, "The http reponse was not correct");
        }
    }
}
