using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net.Sockets;
using Moq;
using MattBarton.NETMF.Utilities.Builders;
using MattBarton.NETMF.Utilities.Adapters;
using MattBarton.NETMF.Utilities.Services;
using MattBarton.NETMF.Utilities.Exceptions.Http;
using MattBarton.NETMF.Utilities.Test.Builders;

namespace MattBarton.NETMF.Utilities.Test.HttpSocket_Tests
{
    [TestFixture]
    class RequestTest
    {
        [Test]
        public void Given_a_host_and_port_are_specified_When_requesting_Then_a_socket_is_connected_to_the_host()
        {
            // setup
            var host = "www.test.com";
            var port = 8080;
            var request = new HttpRequestBuilder()
                .SetUrl(host)
                .SetPort(port)
                .Build();

            var builder = new HttpSocketBuilder()
                .GivenAnHttpRequest();

            var target = builder.Build();

            // execution
            target.Request(request);

            // assertion
            builder.ThenSocketConnectedToHostOnPort(host, port);
        }

        [Test]
        public void Given_a_SocketException_is_thrown_When_requesting_Then_an_HttpSocketConnectionException_is_thrown()
        {
            // setup
            var target = new HttpSocketBuilder()
                .GivenAnHttpRequest()
                .GivenASocketExceptionIsThrown()
                .Build();

            var request = new HttpRequestBuilder()
                .SetUrl("")
                .Build();

            TestDelegate method = () => target.Request(request);

            // execution/assertion
            var exception = Assert.Throws(typeof(HttpSocketConnectionException), method);
            Assert.AreEqual("The socket threw a SocketException",
                exception.Message, "The exception message was not correct");
        }

        [Test]
        public void Given_a_host_is_specified_And_the_host_cannot_be_found_When_requesting_Then_an_HttpUnknownHostException_is_thrown()
        {
            // setup
            var host = "mock host";
            var request = new HttpRequestBuilder()
                .SetUrl(host)
                .Build();

            var target = new HttpSocketBuilder()
                .GivenAnHttpRequest()
                .GivenAHostIsSpecified(host)
                .GivenTheHostCannotBeFound()
                .Build();

            TestDelegate method = () => target.Request(request);

            // execution/assertion
            var exception = Assert.Throws(typeof(HttpUnknownHostException), method);
            Assert.AreEqual("The host mock host could not be found.", exception.Message,
                "The exception message was not correct");
        }

        [Test]
        public void Given_an_HttpRequest_When_requesting_Then_HttpRequest_byte_array_is_sent_to_socket()
        {
            // setup
            var request = new HttpRequestBuilder()
                .SetUrl("www.test.com/test.html")
                .Build();

            var builder = new HttpSocketBuilder();
            var target = builder
                .GivenAnHttpRequest()
                .Build();

            // execution
            target.Request(request);

            // assertion
            builder.ThenHttpRequestByteArrayIsSentToSocket(request.ToByteArray());
        }

        [Test]
        public void Given_a_request_and_no_socket_data_available_and_timeout_not_reached_When_requesting_Then_method_sleeps()
        {
            // setup
            var builder = new HttpSocketBuilder()
                .GivenNoSocketDataAvailable()
                .GivenTimeoutNotReached();

            var target = builder.Build();

            // execution
            target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // assertion
            builder.ThenMethodSleeps();
        }

        [Test]
        public void Given_a_request_and_no_socket_data_available_and_timeout_reached_When_requesting_Then_exception_thrown()
        {
            // setup
            var target = new HttpSocketBuilder()
                .GivenNoSocketDataAvailable()
                .GivenTimeoutReached()
                .Build();

            TestDelegate method = () => target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // execution/assertion
            var exception = Assert.Throws(typeof(HttpTimeoutException), method);
            Assert.AreEqual("The host at www.test.com did not respond in a timely manner.",
                exception.Message, "The exception's message was not correct");
        }

        [Test]
        public void Given_a_request_and_less_than_one_buffers_worth_of_data_available_When_requesting_Then_data_received_once()
        {
            // setup
            var builder = new HttpSocketBuilder()
                .GivenLessThanOneBuffersWorthOfDataAvailable();

            var target = builder.Build();

            // execution
            target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // assertion
            builder.ThenDataReceived(1);
        }

        [Test]
        public void Given_a_request_and_two_buffers_worth_of_data_available_When_requesting_Then_data_received_twice()
        {
            // setup
            var builder = new HttpSocketBuilder()
                .GivenTwoBuffersWorthOfDataAvailable();

            var target = builder.Build();

            // execution
            target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // assertion
            builder.ThenDataReceived(2);
        }

        /*
        [Test]
        public void Given_a_request_and_response_data_returned_When_requesting_Then_response_returned_as_string()
        {
            var chunk = "abcdefghijklmnopqrstuvwxyz";
            var responseText = "";
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    responseText += chunk;
                }
            }

            var target = new HttpSocketBuilder()
                .GivenResponseDataReturned(responseText)
                .Build();

            // execution
            var result = target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // assertion
            Assert.AreEqual(responseText, result, "Result was not correct");
        }
        */
    }
}
