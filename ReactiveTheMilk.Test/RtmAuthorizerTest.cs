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
  public class RtmAuthorizerTest
  {
    private const string ApiKey = "api_key";
    private const string Secret = "signature";
    private const string Frob = "frob";

    private RtmAuthorizer rtm;

    [TestInitialize]
    public void SetUp()
    {
      rtm = new SRtmAuthorizer(ApiKey, Secret);
    }

    [TestMethod]
    [HostType("Moles")]
    public void TestGetFrob()
    {
      var rspRaw = @"<rsp stat=""ok""><frob>frob</frob></rsp>";

      new MRtmBase(rtm).GetRtmResponseString = (method) =>
      {
        method.Is("rtm.auth.getFrob");
        return Observable.Return(rspRaw);
      };

      IObservable<string> result = rtm.GetFrob();
      string frob = result.First();

      frob.Is("frob");
    }

    [TestMethod]
    [HostType("Moles")]
    public void TestGetAuthenticationUrl()
    {
      var called = false;
      var frob = "frob";

      string url = rtm.GetAuthenticationUrl(frob);

      called.Is(true);
      url.Is("http://www.rememberthemilk.com/services/auth/?api_key=api_key&perms=delete&frob=frob&api_sig=signature");
    }

    [TestMethod]
    [HostType("Moles")]
    public void TestGetToken()
    {
      var rspRaw = @"
        <rsp stat=""ok"">
          <auth>
            <token>410c57262293e9d937ee5be75eb7b0128fd61b61</token>
            <perms>delete</perms>
            <user id=""1"" username=""bob"" fullname=""Bob T. Monkey"" />
          </auth>
        </rsp>
      ";

      new MRtmBase(rtm).GetRtmResponseStringParameterArray = (method, parameters) =>
      {
        method.Is("rtm.auth.getToken");
        var param = parameters.First();
        param.Key.Is("frob");
        param.Value.Is("frobvalue");

        return Observable.Return(rspRaw);
      };

      var frob = "frobvalue";
      IObservable<RtmToken> result = rtm.GetToken(frob);
      RtmToken token = result.First();

      token.Token.Is("410c57262293e9d937ee5be75eb7b0128fd61b61");
      token.Perms.Is("delete");
      RtmUser user = token.User;
      user.Id.Is(1);
      user.Username.Is("bob");
      user.Fullname.Is("Bob T. Monkey");
    }

  }
}
