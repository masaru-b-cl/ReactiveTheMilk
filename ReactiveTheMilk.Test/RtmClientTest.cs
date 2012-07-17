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
  public class RtmClientTest
  {
    private const string ApiKey = "api_key";
    private const string Secret = "secret";
    private const string Token = "token";

    private RtmClient rtm;

    [TestInitialize]
    public void SetUp()
    {
      rtm = new SRtmClient(ApiKey, Secret, Token);
    }

    [TestMethod]
    [HostType("Moles")]
    public void TestConstructor()
    {
      rtm.Token.Is("token");
    }

    [TestMethod]
    [HostType("Moles")]
    public void TestGetResponse()
    {
      var rspRaw = @"<rsp stat=""ok""></rsp>";
      new MRtmBase(rtm).GetRtmResponseString = (method) =>
      {
        Assert.Fail();
        return null;
      };

      new MRtmBase(rtm).GetRtmResponseStringParameterArray = (method, parameters) =>
      {
        method.Is("rtm.lists.getList");
        var parameter = parameters.First();
        parameter.Key.Is("auth_token");
        parameter.Value.Is("token");
        return Observable.Return(rspRaw);
      };

      var response = rtm.GetRtmResponse("methodName");
      var result = response.First();
      result.Is(rspRaw);
    }
    
    [TestMethod]
    [HostType("Moles")]
    public void TestGetLists()
    {
      var rspRaw = @"<rsp stat=""ok"">
				<lists>
					<list id=""1"" name=""Inbox""
						deleted=""0"" locked=""0"" archived=""0"" position=""-1"" smart=""0"">
						<filter>(priority:1)</filter>
					</list>
					<list id=""2"" name=""Work""
						deleted=""1"" locked=""1"" archived=""1"" position=""1"" smart=""1"">
						<filter>(priority:1)</filter>
					</list>
				</lists>
			</rsp>";

      new MRtmClient(rtm).GetRtmResponseStringParameterArray = (method, parameters) =>
      {
        method.Is("rtm.lists.getList");
        return Observable.Return(rspRaw);
      };

      IObservable<RtmList> observableList = rtm.GetLists();
      var lists = observableList.ToEnumerable().ToArray();
      {
        var list = lists[0];
        list.Id.Is("1");
        list.Name.Is("Inbox");
        list.Deleted.Is(false);
        list.Locked.Is(false);
        list.Archived.Is(false);
        list.Position.Is(-1);
        list.Smart.Is(false);
        list.Filter.Is("(priority:1)");
      }
      {
        var list = lists[1];
        list.Id.Is("2");
        list.Deleted.Is(true);
        list.Locked.Is(true);
        list.Archived.Is(true);
        list.Position.Is(1);
        list.Smart.Is(true);
      }
    }

  }
}
