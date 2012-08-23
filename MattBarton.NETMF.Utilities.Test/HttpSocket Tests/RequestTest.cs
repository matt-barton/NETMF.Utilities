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

namespace MattBarton.NETMF.Utilities.Test.HttpSocket_Tests
{
    [TestFixture]
    class RequestTest
    {
        [Test]
        public void Given_an_HttpRequest_When_requesting_Then_HttpRequest_byte_array_is_sent_to_socket()
        {
            // setup
            var mockSocket = new Mock<ISocket>();
            var calls = 0;
            mockSocket
                .Setup(s => s.Available())
                .Returns(delegate
                {
                    calls++;
                    return calls == 1 ? 1 : 0;
                });
            
            var request = new HttpRequestBuilder()
                .SetUrl("www.test.com/test.html")
                .SetRequest("the request")
                .Build();

            var target = new HttpSocket(mockSocket.Object, null);

            // execution
            target.Request(request);

            // assertion
            mockSocket.Verify(
                s => s.Send(It.Is<Byte[]>(ba => ba.SequenceEqual(request.ToByteArray()))),
                Times.Once(),
                "Request byte array not sent to socket");
        }

        [Test]
        public void Given_a_request_and_no_socket_data_available_and_timeout_not_reached_When_requesting_Then_method_sleeps()
        {
            // setup
            var mockSocket = new Mock<ISocket>();
            var calls = 0;
            mockSocket
                .Setup(s => s.Available())
                .Returns(delegate
                {
                    calls++;
                    return calls == 1 ? 0 : calls == 2 ? 1 : 0;
                });

            var mockTimerService = new Mock<ITimerService>();
            mockTimerService
                .Setup(ts => ts.TimeoutReached())
                .Returns(false);

            var target = new HttpSocket(mockSocket.Object, mockTimerService.Object);

            // execution
            target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // assertion
            mockTimerService.Verify(
                ts => ts.Sleep(It.Is<int>(i => i == 100)),
                Times.AtLeastOnce(),
                "Method did not sleep");
        }

        [Test]
        public void Given_a_request_and_no_socket_data_available_and_timeout_reached_When_requesting_Then_exception_thrown()
        {
            // setup
            var mockSocket = new Mock<ISocket>();
            mockSocket
                .Setup(s => s.Available())
                .Returns(0);

            var mockTimerService = new Mock<ITimerService>();
            mockTimerService
                .Setup(ts => ts.TimeoutReached())
                .Returns(true);

            var target = new HttpSocket(mockSocket.Object, mockTimerService.Object);

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
            var dataAvailable = (int)Math.Round((double)(HttpSocket.BufferSize / 2));
            var calls = 0;
            var mockSocket = new Mock<ISocket>();
            mockSocket
                .Setup(s => s.Available())
                .Returns(delegate
                {
                    if (calls > 1)
                    {
                        dataAvailable -= HttpSocket.BufferSize;
                    }
                    calls += 1;
                    return dataAvailable < 1 ? 0 : dataAvailable;
                });

            var target = new HttpSocket(mockSocket.Object, null);

            // execution
            target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // assertion
            mockSocket.Verify(
                s => s.Receive(It.Is<int>(i => i == HttpSocket.BufferSize)),
                Times.Once(),
                "Data not received only once");
        }

        [Test]
        public void Given_a_request_and_two_buffers_worth_of_data_available_When_requesting_Then_data_received_twice()
        {
            // setup
            var dataAvailable = HttpSocket.BufferSize * 2;
            var calls = 0;
            var mockSocket = new Mock<ISocket>();
            mockSocket
                .Setup(s => s.Available())
                .Returns(delegate
                {
                    calls += 1;
                    if (calls > 2)
                    {
                        dataAvailable -= HttpSocket.BufferSize;
                    }
                    return dataAvailable < 1 ? 0 : dataAvailable;
                });

            var target = new HttpSocket(mockSocket.Object, null);

            // execution
            target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // assertion
            mockSocket.Verify(
                s => s.Receive(It.Is<int>(i => i == HttpSocket.BufferSize)),
                Times.Exactly(2),
                "Data not received exactly twice");
        }

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

            var mockSocket = new Mock<ISocket>();
            var calls = 0;
            mockSocket
                .Setup(s => s.Available())
                .Returns(delegate
                {
                    calls += 1;
                    if (calls > 2)
                    {
                        var remainder = responseText.Length - ((calls - 2) * HttpSocket.BufferSize);
                        return remainder > 0 ? remainder : 0;
                    }
                    else
                    {
                        return responseText.Length;
                    }
                });

            var responseBytes = Encoding.UTF8.GetBytes(responseText);
            var receiveCalls = 0;
            mockSocket
                .Setup(s => s.Receive(It.Is<int>(i => i == HttpSocket.BufferSize)))
                .Returns(delegate
                {
                    var returnBytes = new Byte[HttpSocket.BufferSize];
                    var y = 0;
                    for (var x = (receiveCalls * HttpSocket.BufferSize); x < ((receiveCalls * HttpSocket.BufferSize) + HttpSocket.BufferSize); x++)
                    {
                        if (x == responseBytes.Length)
                        {
                            break;
                        }
                        returnBytes[y] = responseBytes[x];
                        y++;
                    }
                    if (y < HttpSocket.BufferSize)
                    {
                        var trimmedArray = new Byte[y];
                        trimmedArray = returnBytes.Take(y).ToArray();
                        returnBytes = trimmedArray;
                    }
                    receiveCalls++;
                    return returnBytes;
                });

            var target = new HttpSocket(mockSocket.Object, null);

            // execution
            var result = target.Request(new HttpRequestBuilder()
                .SetUrl("www.test.com")
                .Build());

            // assertion
            Assert.AreEqual(responseText, result, "Result was not correct");
        }
    }
}
