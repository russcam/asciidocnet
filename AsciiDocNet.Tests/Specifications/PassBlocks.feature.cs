﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace AsciiDocNet.Tests.Specifications
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Pass Blocks")]
    public partial class PassBlocksFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PassBlocks.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Pass Blocks", "In order to pass content through unprocessed\r\nAs a writer\r\nI want to be able to m" +
                    "ark passthrough content using a pass block", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Render a pass block without performing substitutions by default to HTML")]
        public virtual void RenderAPassBlockWithoutPerformingSubstitutionsByDefaultToHTML()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Render a pass block without performing substitutions by default to HTML", ((string[])(null)));
#line 8
  this.ScenarioSetup(scenarioInfo);
#line hidden
#line 9
  testRunner.Given("the AsciiDoc source", ":name: value\r\n\r\n++++\r\n<p>{name}</p>\r\n\r\nimage:tiger.png[]\r\n++++", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
  testRunner.When("it is converted to html", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 20
  testRunner.Then("the result should match the HTML source", "<p>{name}</p>\r\n\r\nimage:tiger.png[]", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Render a pass block performing explicit substitutions to HTML")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void RenderAPassBlockPerformingExplicitSubstitutionsToHTML()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Render a pass block performing explicit substitutions to HTML", new string[] {
                        "ignore"});
#line 49
  this.ScenarioSetup(scenarioInfo);
#line hidden
#line 50
  testRunner.Given("the AsciiDoc source", ":name: value\r\n\r\n[subs=\"attributes,macros\"]\r\n++++\r\n<p>{name}</p>\r\n\r\nimage:tiger.pn" +
                    "g[]\r\n++++", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 61
  testRunner.When("it is converted to html", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 62
  testRunner.Then("the result should match the HTML source", "<p>value</p>\r\n\r\n<span class=\"image\"><img src=\"tiger.png\" alt=\"tiger\"></span>", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion