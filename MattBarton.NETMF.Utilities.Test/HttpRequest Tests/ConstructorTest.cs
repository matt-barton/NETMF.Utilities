using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MattBarton.NETMF.Utilities.Test.HttpRequest_Tests
{
    [TestFixture]
    class ConstructorTest
    {
        [Test]
        public void Given_method_is_specified_When_constructing_The_Method_is_set()
        {
            // setup
            var mockMethod = "METHOD";

            // execution
            var result = new HttpRequest("", mockMethod, -1);

            // assertion
            Assert.AreEqual(mockMethod, result.Method, "Method is not correct");
        }

        [Test]
        public void Given_url_is_specified_When_constructing_Then_Hostname_is_set()
        {
            // setup
            var mockUrl = "www.test.com/subdir/resource.html";
            
            // execution
            var result = new HttpRequest(mockUrl, "", -1);

            // assertion
            Assert.AreEqual("www.test.com", result.Hostname, "Hostname is not correct");
        }

        [Test]
        public void Given_url_is_specified_When_constructing_Then_Path_is_set()
        {
            // setup
            var mockUrl = "www.test.com/subdir/resource.html";

            // execution
            var result = new HttpRequest(mockUrl, "", -1);

            // assertion
            Assert.AreEqual("/subdir/resource.html", result.Path, "Path is not correct");
        }

        [Test]
        public void Given_port_is_specified_When_constructing_Then_Port_is_set()
        {
            // setup
            var mockPort = 8080;
            var mockUrl = "test@test.com/";

            // execution
            var result = new HttpRequest(mockUrl, "", mockPort);

            // assertion
            Assert.AreEqual(mockPort, result.Port, "Port is not correct");
        }
    }
}
