using NUnit.Framework;

namespace AsciiDocNet.Tests.Unit
{
	[TestFixture]
	public class LabeledListItemTests
	{
		[Test]
		public void ShouldParseItems()
		{
			var text = @"`ElasticsearchClientException`:: These are known exceptions, either an exception that occurred in the request pipeline
(such as max retries or timeout reached, bad authentication, etc...) or Elasticsearch itself returned an error (could 
not parse the request, bad query, missing field, etc...). If it is an Elasticsearch error, the `ServerError` property 
on the response will contain the the actual error that was returned.  The inner exception will always contain the 
root causing exception.
                                 
`UnexpectedElasticsearchClientException`::  These are unknown exceptions, for instance a response from Elasticsearch not
properly deserialized.  These are usually bugs and {github}/issues[should be reported]. This exception also inherits from `ElasticsearchClientException`
so an additional catch block isn't necessary, but can be helpful in distinguishing between the two.

`Development time exceptions`:: These are CLR exceptions like `ArgumentException`, `ArgumentOutOfRangeException` etc... that are thrown
when an API in the client is misused.  These should not be handled as you want to know about them during development.";

			var document = Document.Parse(text);

			Assert.IsNotNull(document);
			Assert.IsTrue(document.Count == 1);
			Assert.IsInstanceOf<LabeledList>(document[0]);

			var labeledList = (LabeledList)document[0];

			Assert.IsTrue(labeledList.Items.Count == 3);
		}
	}
}