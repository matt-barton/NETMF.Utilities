using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MattBarton.NETMF.Utilities.Test.HttpClientTests
{
	[TestFixture]
	class getTest
	{
		[Test]
		public void When_blahblahblah()
		{
			var target = new HttpClient();
			var result = target.get("");

			Assert.AreEqual(result, "response", "resonse is not correct");
		}
	}
}
