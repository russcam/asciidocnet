using System.IO;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AsciiDocNet.Tests.Specifications
{
    [Binding]
    public class Steps
    {
        [Given(@"the AsciiDoc source")]
        public void GivenTheAsciiDocSource(string text)
        {
	        var document = Document.Parse(text);
            ScenarioContext.Current.Add("document", document);
        }
        
        [When(@"it is converted to html")]
        public void WhenItIsConvertedToHtml()
        {
	        var source = ScenarioContext.Current.Get<Document>("document");
			var builder = new StringBuilder();

			using (var visitor = new HtmlSectionVisitor(new StringWriter(builder)))
			{
				source.Accept(visitor);
			}

	        var output = builder.ToString().TrimEnd('\r', '\n');

			ScenarioContext.Current.Add("output", output);
        }
        
        [Then(@"the result should match the HTML source")]
        public void ThenTheResultShouldMatchTheHTMLSource(string text)
        {
	        var output = ScenarioContext.Current.Get<string>("output");
			Assert.AreEqual(text, output);
        }
    }
}
