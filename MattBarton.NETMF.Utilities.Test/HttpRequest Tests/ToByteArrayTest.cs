using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MattBarton.NETMF.Utilities.Builders;
using MattBarton.NETMF.Utilities.Enumerations;

namespace MattBarton.NETMF.Utilities.Test.HttpRequest_Tests
{
    [TestFixture]
    class ToByteArrayTest
    {
        [Test]
        public void Given_request_is_set_When_ToByteArray_called_Then_request_byte_array_returned()
        {
            // setup
            var method = "GET";
            var hostname = "www.test.com";
            var path = "/test.php";
            var url = hostname + path;
            var port = 8081;

            var target = new HttpRequest(url, HttpMethod.GET, port);

            // execution
            var result = target.ToByteArray();

            var assembledRequest = new StringBuilder();
            assembledRequest.Append(method);
            assembledRequest.Append(" ");
            assembledRequest.Append(path);
            assembledRequest.Append(" ");
            assembledRequest.Append("HTTP/1.0\nHost: ");
            assembledRequest.Append(hostname);
            assembledRequest.Append("\nConnection: Close\n\n");

            // assertion
            Assert.AreEqual(
                Encoding.UTF8.GetBytes(assembledRequest.ToString()),
                result,
                "Byte array is not correct");
        }
    }
}
