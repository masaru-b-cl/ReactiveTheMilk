using ReactiveTheMilk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ReactiveTheMilk
{
  [TestClass()]
  public class RtmUtilsTest
  {
    [TestMethod]
    public void TestGenerateSignature()
    {
      var method = new Parameter("method", "rtm");
      var api_key = new Parameter("api_key", "api_key");
      var parameters = new Parameter[] { method, api_key };
      string secret = "secret";
      String signature = RtmUtils.GenerateSignature(parameters, secret);

      signature.Is("1770f4c43928e83b2c301ead6604464d");
    }
  }
}
