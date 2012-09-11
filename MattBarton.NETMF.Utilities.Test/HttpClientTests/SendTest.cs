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
using MattBarton.NETMF.Utilities.Builders;
using MattBarton.NETMF.Utilities.Enumerations;

namespace MattBarton.NETMF.Utilities.Test.HttpClientTests
{
	/// <summary>
	/// Tests for the Send method of the HttpClient
	/// </summary>
	[TestFixture]
	class SendTest
	{
        [Test]
        public void Given_url_and_port_and_arguments_are_set_When_sending_Then_request_is_sent_via_socket()
        {
            // setup
            var mockHostname = "www.test.com";
            var mockPath = "/resource.html";
            var mockUrl = mockHostname + mockPath;
            var mockPort = 8080;
            var mockSocket = new Mock<IHttpSocket>();

            var mockRequest = new HttpRequestBuilder()
                .SetUrl(mockUrl)
                .SetPort(mockPort)
                .Build();

            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(200)
                .SetResponse("response")
                .Build();

            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution
            target.Send(mockRequest);

            // assertion
            mockSocket.Verify(
                s => s.Request(It.Is<HttpRequest>(
                    hr => hr.Hostname == mockHostname &&
                        hr.Path == mockPath &&
                        hr.Port == mockPort &&
                        hr.Method == HttpMethod.GET)),
                Times.Once(),
                "Request not sent via socket");
        }

        [Test]
        public void Given_that_http_reponse_is_not_valid_When_sending_Then_HttpInvalidResponseException_thrown()
        {
            var mockResponse = "";

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution/assertion
            TestDelegate testMethod = () => target.Send(new HttpRequest(""));
            Assert.Throws(typeof(HttpInvalidResponseException), testMethod);
        }


        [Test]
        public void Given_that_http_status_code_1xx_is_returned_When_sending_Then_HttpUnhandledStatusException_thrown()
        {
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(100)
                .SetStatusDescriptor("Redirection")
                .SetResponse("Some response")
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var target = new HttpClient(mockSocket.Object);

            // execution/assertion
            TestDelegate testMethod = () => target.Send(new HttpRequest(""));
            var exception = Assert.Throws(typeof(HttpRequestException), testMethod);
            Assert.AreEqual("100 Redirection", exception.Message, "Exception message is not correct");
        }

        [Test]
        public void Given_that_http_status_code_3xx_is_returned_When_sending_Then_HttpUnhandledRedirectionException_thrown()
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
            TestDelegate testMethod = () => target.Send(new HttpRequest(""));
            var exception = Assert.Throws(typeof(HttpRequestException), testMethod);
            Assert.AreEqual("301 Moved Permanently", exception.Message, "Exception message was not correct");
        }

        [Test]
        public void Given_that_http_status_code_4xx_is_returned_When_sending_Then_HttpClientErrorException_thrown()
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
            TestDelegate testMethod = () => target.Send(new HttpRequest(""));
            var exception = Assert.Throws(typeof(HttpRequestException), testMethod);
            Assert.AreEqual("404 Not Found", exception.Message, "Exception message was not correct");
        }

        [Test]
        public void Given_that_http_status_code_5xx_is_returned_When_sending_Then_HttpServerErrorException_thrown()
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
            TestDelegate testMethod = () => target.Send(new HttpRequest(""));
            var exception = Assert.Throws(typeof(HttpRequestException), testMethod);
            Assert.AreEqual("500 Server Error", exception.Message, "Exception message was not correct");
        }

        [Test]
        public void Given_that_http_status_code_2xx_is_returned_When_sending_Then_response_is_returned()
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
            var result = target.Send(new HttpRequest(""));

            // assertion
            Assert.AreEqual(content, result, "The http reponse was not correct");
        }

        [Test]
        public void Given_that_http_status_code_2xx_And_http_headers_are_returned_When_sending_Then_response_is_returned()
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
            var result = target.Send(new HttpRequest(""));

            // assertion
            Assert.AreEqual(content, result, "The http reponse was not correct");
        }

        [Test]
        public void Given_that_request_is_present_When_sending_Then_request_status_code_is_set()
        {
            var content = "some response content";
            var statusCode = 201;
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(statusCode)
                .SetStatusDescriptor("OK")
                .SetResponse(content)
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var mockRequest = new HttpRequest("");

            var target = new HttpClient(mockSocket.Object);

            // execution
            
            var result = target.Send(mockRequest);

            // assertion
            Assert.AreEqual(statusCode, mockRequest.StatusCode, "The request status code is not correct");
        }

        [Test]
        public void Given_that_request_is_present_and_status_code_indicates_success_When_sending_Then_request_Success_is_set_true()
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

            var mockRequest = new HttpRequest("");

            var target = new HttpClient(mockSocket.Object);

            // execution

            var result = target.Send(mockRequest);

            // assertion
            Assert.IsTrue(mockRequest.Success, "The request success property is not correct");
        }

        [Test]
        public void Given_that_request_is_present_and_status_code_indicates_error_When_sending_Then_request_Success_is_set_false()
        {
            var content = "some response content";
            var mockResponse = new HttpResponseBuilder()
                .SetStatusCode(404)
                .SetStatusDescriptor("OK")
                .SetResponse(content)
                .Build();

            var mockSocket = new Mock<IHttpSocket>();
            mockSocket
                .Setup(s => s.Request(It.IsAny<HttpRequest>()))
                .Returns(mockResponse);

            var mockRequest = new HttpRequest("");

            var target = new HttpClient(mockSocket.Object);

            // execution/assertion
            TestDelegate method = () => target.Send(mockRequest);
            var exception = (HttpRequestException)Assert.Throws(typeof (HttpRequestException), method,
                "Correct exception not thrown");

            Assert.IsFalse(exception.Request.Success, "The request success property is not correct");
        }

    }
}
