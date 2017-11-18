//using System;
//using System.IO;
//using System.Text;
//using AsciiDocNet.Tests.Extensions;
//using TechTalk.SpecFlow;
//using Xunit;

//namespace AsciiDocNet.Tests.Specifications
//{
//    [Binding]
//    public class Steps : TechTalk.SpecFlow.Steps
//    {
//        [Given(@"the AsciiDoc source")]
//        public void GivenTheAsciiDocSource(string text)
//        {
//	        var document = Document.Parse(text);
//            this.ScenarioContext.Add("document", document);
//        }
        
//        [When(@"it is converted to html")]
//        public void WhenItIsConvertedToHtml()
//        {
//	        var source = this.ScenarioContext.Get<Document>("document");
//			var builder = new StringBuilder();

//			using (var visitor = new HtmlSectionVisitor(new StringWriter(builder)))
//			{
//				source.Accept(visitor);
//			}

//	        var output = builder.ToString().RemoveTrailingNewLine();
//			this.ScenarioContext.Add("output", output);
//        }
        
//        [Then(@"the result should match the HTML source")]
//        public void ThenTheResultShouldMatchTheHtmlSource(string text)
//        {
//	        var output = this.ScenarioContext.Get<string>("output");
//	        text = text.ConsistentLineEndings();
//	        output = output.ConsistentLineEndings();
//			Assert.Equal(text, output);
//        }
//    }
//}
