using MattBarton.NETMF.Utilities.Adapters;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using MattBarton.NETMF.Utilities.Exceptions.Http;
using MattBarton.NETMF.Utilities.Test.Builders;

namespace MattBarton.NETMF.Utilities.Test.HttpSocket_Tests
{
    [TestFixture]
    class ConnectTest
    {
        [Test]
        public void Given_a_host_and_port_are_specified_When_connecting_Then_a_socket_is_connected_to_the_host()
        {
            // setup
            var host = "www.test.com";
            var port = 8080;

            var builder = new HttpSocketBuilder(); 
            var target = builder.Build();

            // execution
            target.Connect(host, port);

            // assertion
            builder.ThenSocketConnectedToHostOnPort(host, port);
        }

        [Test]
        public void Given_a_SocketException_is_thrown_When_connecting_Then_an_HttpSocketConnectionException_is_thrown()
        {
            // setup
            var target = new HttpSocketBuilder()
                .GivenASocketExceptionIsThrown()
                .Build();

            TestDelegate method = () => target.Connect("", 8080);

            // execution/assertion
            var exception = Assert.Throws(typeof(HttpSocketConnectionException), method);
            Assert.AreEqual("The socket threw a SocketException",
                exception.Message, "The exception message was not correct");
        }

        [Test]
        public void Given_a_host_is_specified_And_the_host_cannot_be_found_When_connecting_Then_an_HttpUnknownHostException_is_thrown()
        {
            // setup
            var host = "mock host";

            var target = new HttpSocketBuilder()
                .GivenAHostIsSpecified(host)
                .GivenTheHostCannotBeFound()
                .Build();

            TestDelegate method = () => target.Connect(host, -1);

            // execution/assertion
            var exception = Assert.Throws(typeof(HttpUnknownHostException), method);
            Assert.AreEqual("The host mock host could not be found.", exception.Message,
                "The exception message was not correct");
        }
    }
}
