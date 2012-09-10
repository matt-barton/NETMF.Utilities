using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MattBarton.NETMF.Utilities.Test.HttpRequest_Tests
{
    [TestFixture]
    class AddArgumentTest
    {
        [Test]
        public void Given_argument_name_and_value_are_defined_When_adding_argument_Then_argument_added()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;
            var mockArgumentName = "ArgumentName";
            var mockArgumentValue = "ArgumentValue";

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("GET " + mockPath + "?" + mockArgumentName + "=" + mockArgumentValue + " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\n\n");
            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl);

            // execution
            target.AddArgument(mockArgumentName, mockArgumentValue);

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not added");
        }

        [Test]
        public void Given_argument_name_is_defined_And_argument_value_is_not_defined_When_adding_argument_Then_argument_added()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;
            var mockArgumentName = "ArgumentName";

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("GET " + mockPath + "?" + mockArgumentName + "= HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\n\n");
            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl);

            // execution
            target.AddArgument(mockArgumentName, "");

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not added");
        }

        [Test]
        public void Given_argument_name_is_not_defined_When_adding_argument_Then_argument_not_added()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("GET " + mockPath + " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\n\n");
            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl);

            // execution
            target.AddArgument("", "");

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not ignored");
        }

        [Test]
        public void Given_argument_name_and_value_are_defined_And_have_encodable_characters_When_adding_argument_Then_argument_added_with_encodable_characters_encoded()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;
            var mockArgumentName = "Argument Name";
            var mockArgumentValue = "Argument&Value";
            var encodedArgumentName = "Argument%20Name";
            var encodedArgumentValue = "Argument%26Value";

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("GET " + mockPath + "?" + encodedArgumentName + "=" + encodedArgumentValue + " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\n\n");
            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl);

            // execution
            target.AddArgument(mockArgumentName, mockArgumentValue);

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not added");
        }

    }
}
