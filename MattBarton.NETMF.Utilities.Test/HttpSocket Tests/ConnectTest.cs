using MattBarton.NETMF.Utilities.Adapters;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using MattBarton.NETMF.Utilities.Exceptions.Http;

namespace MattBarton.NETMF.Utilities.Test.HttpSocket_Tests
{
    [TestFixture]
    class ConnectTest
    {
        [Test]
        public void Given_a_host_and_port_are_specified_When_connecting_Then_a_socket_is_connected_to_the_host()
        {
            // setup
            var mockSocket = new Mock<ISocket>();
            var host = "www.test.com";
            var port = 8080;

            var target = new HttpSocket(mockSocket.Object, null);

            // execution
            target.Connect(host, port);

            // assertion
            mockSocket.Verify(
                s => s.Connect(It.Is<EndPoint>(ep => ep.Equals(
                    new IPEndPoint(Dns.GetHostEntry(host).AddressList[0], port)))),
                Times.Once(),
                "The socket was not connected");
            
        }

        [Test]
        public void Given_a_SocketException_is_thrown_When_connecting_Then_an_HttpSocketConnectionException_is_thrown()
        {
            // setup
            var mockException = new SocketException();
            var mockSocket = new Mock<ISocket>();
            mockSocket
                .Setup(s => s.Connect(It.IsAny<IPEndPoint>()))
                .Throws(mockException);

            var target = new HttpSocket(mockSocket.Object, null);

            TestDelegate method = () => target.Connect("", 8080);

            // execution/assertion
            var exception = Assert.Throws(typeof(HttpSocketConnectionException), method);
            Assert.AreEqual(mockException,
                exception.InnerException, "The innerException was not correct");
            Assert.AreEqual("The socket threw a SocketException",
                exception.Message, "The exception message was not correct");
        }
    }
}
