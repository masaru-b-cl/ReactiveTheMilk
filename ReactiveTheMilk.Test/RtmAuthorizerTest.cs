using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTheMilk
{
  [TestClass]
  public class RtmAuthorizerTest
  {
    private const string ApiKey = "api_key";
    private const string Secret = "signature";
    private const string Frob = "frob";

    private RtmAuthorizer rtm;

    [TestInitialize]
    public void SetUp()
    {
      rtm = new RtmAuthorizer(ApiKey, Secret);
    }

    [TestMethod()]
    public void TestGenerateSignature()
    {
      var method = new Parameter("method", "rtm");
      var api_key = new Parameter("api_key", ApiKey);
      var parameters = new Parameter[] { method, api_key };
      String signature = rtm.GenerateSignature(parameters);

      Assert.AreEqual(signature, "2210109e7b5762b07190d4fb5abc68c7");
    }
  }
}
