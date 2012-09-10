using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MattBarton.NETMF.Utilities.Enumerations;

namespace MattBarton.NETMF.Utilities.Test.HttpRequest_Tests
{
    [TestFixture]
    class AddArgumentTest
    {
        [Test]
        public void Given_argument_name_and_value_are_defined_And_method_is_GET_When_adding_argument_Then_argument_added()
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
        public void Given_nultiple_argument_names_and_values_are_defined_And_method_is_GET_When_adding_argument_Then_all_arguments_added()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;
            var mockArgumentName1 = "ArgumentName";
            var mockArgumentValue1 = "ArgumentValue";
            var mockArgumentName2 = "SecondArgumentName";
            var mockArgumentValue2 = "SecondArgumentValue";
            var mockArgumentName3 = "ArgumentNameThree";
            var mockArgumentValue3 = "ArgumentValueThree";

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("GET " + mockPath + 
                "?" + mockArgumentName1 + "=" + mockArgumentValue1 +
                "&" + mockArgumentName2 + "=" + mockArgumentValue2 +
                "&" + mockArgumentName3 + "=" + mockArgumentValue3 +
                " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\n\n");
            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl);

            // execution
            target.AddArgument(mockArgumentName1, mockArgumentValue1);
            target.AddArgument(mockArgumentName2, mockArgumentValue2);
            target.AddArgument(mockArgumentName3, mockArgumentValue3);

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not added");
        }

        [Test]
        public void Given_argument_name_is_defined_And_argument_value_is_not_defined_And_method_is_GET_When_adding_argument_Then_argument_added()
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
        public void Given_argument_name_is_not_defined_And_method_is_GET_When_adding_argument_Then_argument_not_added()
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
        public void Given_argument_name_and_value_are_defined_And_have_encodable_characters_And_method_is_GET_When_adding_argument_Then_argument_added_with_encodable_characters_encoded()
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

        [Test]
        public void Given_argument_name_and_value_are_defined_And_method_is_POST_When_adding_argument_Then_argument_added()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;
            var mockArgumentName = "ArgumentName";
            var mockArgumentValue = "ArgumentValue";

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("POST " + mockPath + " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\nContent-Type: application/x-www-form-urlencoded");
            assembledRequest.Append("\nContent-Length: " + 
                (mockArgumentName.Length + mockArgumentValue.Length + 1).ToString());
            assembledRequest.Append("\n\n");
            assembledRequest.Append(mockArgumentName + "=" + mockArgumentValue);

            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl, HttpMethod.POST);

            // execution
            target.AddArgument(mockArgumentName, mockArgumentValue);

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not added");
        }

        [Test]
        public void Given_argument_name_is_defined_And_argument_value_is_not_defined_And_method_is_POST_When_adding_argument_Then_argument_added()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;
            var mockArgumentName = "ArgumentName";

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("POST " + mockPath + " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\nContent-Type: application/x-www-form-urlencoded");
            assembledRequest.Append("\nContent-Length: " +
                (mockArgumentName.Length + 1).ToString());
            assembledRequest.Append("\n\n");
            assembledRequest.Append(mockArgumentName + "=");

            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl, HttpMethod.POST);

            // execution
            target.AddArgument(mockArgumentName, "");

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not added");
        }

        [Test]
        public void Given_argument_name_is_not_defined_And_method_is_POST_When_adding_argument_Then_argument_not_added()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("POST " + mockPath + " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\nContent-Type: application/x-www-form-urlencoded");
            assembledRequest.Append("\nContent-Length: 0");
            assembledRequest.Append("\n\n");
            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl, HttpMethod.POST);

            // execution
            target.AddArgument("", "");

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not ignored");
        }

        [Test]
        public void Given_argument_name_and_value_are_defined_And_have_encodable_characters_And_method_is_POST_When_adding_argument_Then_argument_added_with_encodable_characters_encoded()
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
            assembledRequest.Append("POST " + mockPath + " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\nContent-Type: application/x-www-form-urlencoded");
            assembledRequest.Append("\nContent-Length: " +
                (encodedArgumentName.Length + encodedArgumentValue.Length + 1).ToString());
            assembledRequest.Append("\n\n");
            assembledRequest.Append(encodedArgumentName + "=" + encodedArgumentValue);

            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl, HttpMethod.POST);

            // execution
            target.AddArgument(mockArgumentName, mockArgumentValue);

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not added");
        }

        [Test]
        public void Given_nultiple_argument_names_and_values_are_defined_And_method_is_POST_When_adding_argument_Then_all_arguments_added()
        {
            // setup
            var mockHost = "www.test.com";
            var mockPath = "/index.html";
            var mockUrl = mockHost + mockPath;
            var mockArgumentName1 = "ArgumentName";
            var mockArgumentValue1 = "ArgumentValue";
            var mockArgumentName2 = "SecondArgumentName";
            var mockArgumentValue2 = "SecondArgumentValue";
            var mockArgumentName3 = "ArgumentNameThree";
            var mockArgumentValue3 = "ArgumentValueThree";

            var assembledRequest = new StringBuilder();
            assembledRequest.Append("POST " + mockPath + " HTTP/1.0\n");
            assembledRequest.Append("Host: ");
            assembledRequest.Append(mockHost);
            assembledRequest.Append("\nConnection: Close");
            assembledRequest.Append("\nContent-Type: application/x-www-form-urlencoded");
            assembledRequest.Append("\nContent-Length: " + (
                mockArgumentName1.Length + mockArgumentValue1.Length + 1 +
                mockArgumentName2.Length + mockArgumentValue2.Length + 2 +
                mockArgumentName3.Length + mockArgumentValue3.Length + 2).ToString());
            assembledRequest.Append("\n\n");
            assembledRequest.Append(mockArgumentName1 + "=" + mockArgumentValue1 +
                "&" + mockArgumentName2 + "=" + mockArgumentValue2 +
                "&" + mockArgumentName3 + "=" + mockArgumentValue3);

            var expectedBytes = Encoding.UTF8.GetBytes(assembledRequest.ToString());

            var target = new HttpRequest(mockUrl, HttpMethod.POST);

            // execution
            target.AddArgument(mockArgumentName1, mockArgumentValue1);
            target.AddArgument(mockArgumentName2, mockArgumentValue2);
            target.AddArgument(mockArgumentName3, mockArgumentValue3);

            // assertion
            var result = target.ToByteArray();
            Assert.AreEqual(expectedBytes, result, "Argument not added");
        }
    }
}
