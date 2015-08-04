using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NUnitReportMerge
{
  public class TestSuiteBuilder
  {
    private readonly IEnumerable<XElement> _assemblyResults;
    string Result { get; set; }
    double Time { get; set; }
    int Asserts { get; set; }

    public TestSuiteBuilder(string result, double time, int asserts, IEnumerable<XElement> assemblyResults)
    {
      _assemblyResults = assemblyResults;
      Result = result;
      Time = time;
      Asserts = asserts;
    }

    public string CreateOutResult(XElement xElement)
    {
      string outResult;
      if (xElement.Attribute("result").Value == "Failure")
      {
        outResult = "Failure";
      }
      else if (xElement.Attribute("result").Value == "Inconclusive" && Result == "Success")
      {
        outResult = "Inconclusive";
      }
      else
      {
        outResult = Result;
      }
      return outResult;
    }

    public void Add(XElement xElement)
    {
      Result = CreateOutResult(xElement);
      Time += XmlCulture.GetDouble(xElement.Attribute("time").Value);
      Asserts += XmlCulture.GetInt(xElement.Attribute("asserts").Value);
    }

    public XElement Build()
    {
      var results = AddResultsTag();
      var projectXml = CreateTestSuiteTag();
      projectXml.Add(results);
      return projectXml;
    }

    XElement AddResultsTag()
    {
      var results = XElement.Parse("<results/>");
      results.Add(_assemblyResults.ToArray());
      return results;
    }

    XElement CreateTestSuiteTag()
    {
      return XElement.Parse(
        string.Format(CultureInfo.InvariantCulture,
          "<test-suite type=\"Test Project\" name=\"\" executed=\"True\" result=\"{0}\" time=\"{1:F6}\" asserts=\"{2}\" />",
          Result,
          Time,
          Asserts));
    }
  }
}