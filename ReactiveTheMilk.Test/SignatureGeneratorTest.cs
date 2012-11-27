using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTheMilk
{
  [TestClass]
  public class SignatureGeneratorTest
  {
    [TestMethod]
    public void TestConstructor()
    {
      string secret = "secret";
      var generator = new SignatureGenerator(secret).AsDynamic();

      (generator.secret as string).Is("secret");
    }

    [TestMethod]
    public void TestGenerateSignature()
    {
      var method = new Parameter("method", "rtm");
      var api_key = new Parameter("api_key", "api_key");
      var parameters = new Parameter[] { method, api_key };
      var secret = "secret";
      var generator = new SignatureGenerator(secret);

      String signature = generator.Generate(parameters);

      signature.Is("1770f4c43928e83b2c301ead6604464d");
    }
  }
}
