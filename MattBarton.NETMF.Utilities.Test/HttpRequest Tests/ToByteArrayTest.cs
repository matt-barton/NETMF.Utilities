using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MattBarton.NETMF.Utilities.Builders;

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
            var request = "this is the request";

            var target = new HttpRequest(method, url, port, request);

            // execution
            var result = target.ToByteArray();

            var assembledRequest = new StringBuilder();
            assembledRequest.Append(method);
            assembledRequest.Append(" ");
            assembledRequest.Append(path);
            assembledRequest.Append(" ");
            assembledRequest.Append("HTTP/1.1\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(hostname);
            assembledRequest.Append("\n\n");
            assembledRequest.Append(request);

            // assertion
            Assert.AreEqual(
                Encoding.UTF8.GetBytes(assembledRequest.ToString()),
                result,
                "Byte array is not correct");
        }
    }
}
