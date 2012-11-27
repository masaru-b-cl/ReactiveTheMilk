using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using System.Net;
using ReactiveTheMilk.Moles;
using System.Net.Moles;
using Codeplex.Reactive.Asynchronous.Moles;

namespace ReactiveTheMilk
{
  [TestClass]
  public class RtmBaseTest
  {
    private const string ApiKey = "api_key";
    private const string Secret = "secret";

    private RtmBase rtm;

    [TestInitialize]
    public void SetUp()
    {
      rtm = new SRtmBase(ApiKey, Secret);
    }

    [TestMethod]
    [HostType("Moles")]
    public void TestGetRtmResponse()
    {
      bool called = false;
      MRtmBase.AllInstances.GetRtmResponseStringParameterArray =
        (r, method, parameters) =>
        {
          called = true;
          method.Is("method");
          parameters.Count().Is(0);
          return Observable.Return(@"<rsp stat=""ok""></rsp>");
        };

      string rspRaw = rtm.GetRtmResponse("method").First();

      called.Is(true);
      rspRaw.Is(@"<rsp stat=""ok""></rsp>");
    }

    [TestMethod]
    [HostType("Moles")]
    public void TestGetRtmResponseWithParameters()
    {

      MParametersExtension.ToPostDataIEnumerableOfParameter =
        parameters =>
        {
          var array = parameters.ToArray();
          array[0].Key.Is("key");
          array[0].Value.Is("value");

          array[1].Key.Is("method");
          array[1].Value.Is("method");

          array[2].Key.Is("api_key");
          array[2].Value.Is(ApiKey);

          array[3].Key.Is("api_sig");
          array[3].Value.Is("signature");

          return "parameters";
        };

      var response = new SWebResponse();
      MWebRequestExtensions.UploadStringAsyncWebRequestStringEncoding =
        (request, param, encoding) =>
        {
          request.RequestUri.OriginalString.Is("https://api.rememberthemilk.com/services/rest/");
          request.Method.Is("POST");
          request.ContentType.Is("application/x-www-form-urlencoded");

          param.Is("parameters");
          encoding.Is(Encoding.UTF8);

          return Observable.Return(response);
        };

      MWebResponseExtensions.DownloadStringAsyncWebResponseEncoding =
        (res, encoding) =>
        {
          res.Is(response);
          encoding.Is(Encoding.UTF8);

          return Observable.Return(@"<rsp stat=""ok""></rsp>");
        };

      string rspRaw = rtm.GetRtmResponse("method", new[] { new Parameter("key", "value") }).First();

      rspRaw.Is(@"<rsp stat=""ok""></rsp>");
    }

    [TestMethod]
    [HostType("Moles")]
    public void TestGetRtmResponseWhenFail()
    {
      MWebRequestExtensions.UploadStringAsyncWebRequestStringEncoding =
        (req, param, encoding) =>
        {
          return Observable.Return(new SWebResponse());
        };

      MWebResponseExtensions.DownloadStringAsyncWebResponseEncoding =
        (res, encoding) =>
        {
          return Observable.Return(@"<rsp stat=""fail""><err code=""112"" msg=""Method &quot;rtm.auth.getFrobs&quot; not found""/></rsp>");
        };

      bool catched = false;
      rtm.GetRtmResponse("method")
        .Catch((RtmException ex) =>
        {
          catched = true;
          ex.Code.Is("112");
          ex.Msg.Is(@"Method ""rtm.auth.getFrobs"" not found");
          return Observable.Return("");
        }).First();

      if (!catched)
      {
        Assert.Fail("例外をキャッチしていない");
      }
    }
  }
}
