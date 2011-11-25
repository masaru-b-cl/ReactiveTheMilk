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

	}
}
