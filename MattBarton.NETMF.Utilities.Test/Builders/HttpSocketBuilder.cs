using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MattBarton.UnitTesting;
using Moq;
using MattBarton.NETMF.Utilities.Adapters;
using MattBarton.NETMF.Utilities.Services;
using System.Net.Sockets;
using MattBarton.NETMF.Utilities.Exceptions.Http;
using System.Net;

namespace MattBarton.NETMF.Utilities.Test.Builders
{
    /// <summary>
    /// Builder utility for HttpSocket unit test cases
    /// </summary>
    class HttpSocketBuilder : IBuilder<HttpSocket>
    {
        #region Fields

        private string _host;
        private Mock<ISocket> _socket = new Mock<ISocket>();
        private Mock<ITimerService> _timerService = new Mock<ITimerService>();
        private Mock<IDns> _dns;

        #endregion

        #region IBuilder Methods

        public HttpSocket Build()
        {
            return new HttpSocket(
                this._socket.Object, 
                this._timerService.Object,
                this._dns != null ? this._dns.Object : null);
        }

        #endregion

        #region Givens

        public HttpSocketBuilder GivenAHostIsSpecified(string host)
        {
            this._host = host;
            return this;
        }

        public HttpSocketBuilder GivenTheHostCannotBeFound()
        {
            var mockException = new SocketException();

            this._dns = new Mock<IDns>();
            this._dns
                .Setup(dns => dns.GetHostEntry(It.IsAny<string>()))
                .Throws(mockException);

            return this;
        }

        public HttpSocketBuilder GivenASocketExceptionIsThrown()
        {
            var mockException = new SocketException();
            
            this._socket
                .Setup(s => s.Connect(It.IsAny<IPEndPoint>()))
                .Throws(mockException);

            return this;
        }

        public HttpSocketBuilder GivenAnHttpRequest()
        {
            var calls = 0;
            this._socket
                .Setup(s => s.Available())
                .Returns(delegate
                {
                    calls++;
                    return calls == 1 ? 1 : 0;
                });

            return this;
        }

        public HttpSocketBuilder GivenNoSocketDataAvailable()
        {
            var calls = 0;
            this._socket
                .Setup(s => s.Available())
                .Returns(delegate
                {
                    calls++;
                    return calls == 1 ? 0 : calls == 2 ? 1 : 0;
                });

            return this;
        }

        public HttpSocketBuilder GivenLessThanOneBuffersWorthOfDataAvailable()
        {
            var dataAvailable = (int)Math.Round((double)(HttpSocket.BufferSize / 2));
            var calls = 0;
            this._socket
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

            return this;
        }

        public HttpSocketBuilder GivenTwoBuffersWorthOfDataAvailable()
        {
            var dataAvailable = HttpSocket.BufferSize * 2;
            var calls = 0;
            this._socket
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

            return this;
        }

        public HttpSocketBuilder GivenResponseDataReturned(string responseText)
        {
            var calls = 0;
            this._socket
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
            this._socket
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

            return this;
        }

        public HttpSocketBuilder GivenTimeoutReached()
        {
            this._timerService
                .Setup(ts => ts.TimeoutReached())
                .Returns(true);

            return this;
        }

        public HttpSocketBuilder GivenTimeoutNotReached()
        {
            this._timerService
                .Setup(ts => ts.TimeoutReached())
                .Returns(false);

            return this;
        }

        #endregion

        #region Thens

        public void ThenSocketConnectedToHostOnPort(string host, int port)
        {
            this._socket.Verify(
                s => s.Connect(It.Is<EndPoint>(ep => ep.Equals(
                    new IPEndPoint(Dns.GetHostEntry(host).AddressList[0], port)))),
                Times.Once(),
                "The socket was not connected");
        }

        public void ThenHttpRequestByteArrayIsSentToSocket(Byte[] bytes)
        {
            this._socket.Verify(
                s => s.Send(It.Is<Byte[]>(ba => ba.SequenceEqual(bytes))),
                Times.Once(),
                "Request byte array not sent to socket");
        }

        public void ThenMethodSleeps()
        {
            this._timerService.Verify(
                ts => ts.Sleep(It.Is<int>(i => i == 100)),
                Times.AtLeastOnce(),
                "Method did not sleep");
        }

        public void ThenDataReceived(int times)
        {
            this._socket.Verify(
                s => s.Receive(It.Is<int>(i => i == HttpSocket.BufferSize)),
                Times.Exactly(times),
                "Data not received exactly " + times + " time(s)");
        }
        #endregion
    }
}
