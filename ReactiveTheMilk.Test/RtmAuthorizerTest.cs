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

			MRtmBase.AllInstances.GetRtmResponseString = (a, method) =>
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

      MRtmUtils.GenerateSignatureIEnumerableOfParameterString = (parameters, secret) =>
        {
          var paramarray = parameters.ToArray();
          {
            var parameter = paramarray[0];
            parameter.Key.Is("api_key");
            parameter.Value.Is(ApiKey);
          }
          {
            var parameter = paramarray[1];
            parameter.Key.Is("perms");
            parameter.Value.Is("delete");
          }
          {
            var parameter = paramarray[2];
            parameter.Key.Is("frob");
            parameter.Value.Is(frob);
          }
          called = true;
          return "signature";
        };

      string url = rtm.GetAuthenticationUrl(frob);

      called.Is(true);
      url.Is("http://www.rememberthemilk.com/services/auth/?api_key=api_key&perms=delete&frob=frob&api_sig=signature");
    }

	}
}
