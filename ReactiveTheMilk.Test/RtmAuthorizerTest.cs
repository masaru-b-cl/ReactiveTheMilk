using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using System.Net;
using ReactiveTheMilk.Moles;
using System.Net.Moles;


namespace ReactiveTheMilk
{
	[TestClass]
	public class RtmAuthorizerTest
	{
		private const string ApiKey = "api_key";
		private const string Secret = "signature";
		private const string Frob = "frob";

		private SRtmAuthorizer rtm;

		[TestInitialize]
		public void SetUp()
		{
			rtm = new SRtmAuthorizer(ApiKey, Secret);
		}

		[TestMethod]
		public void TestGenerateSignature()
		{
			var method = new Parameter("method", "rtm");
			var api_key = new Parameter("api_key", ApiKey);
			var parameters = new Parameter[] { method, api_key };
			String signature = rtm.GenerateSignature(parameters);

			signature.Is("2210109e7b5762b07190d4fb5abc68c7");
		}

		[TestMethod]
		[HostType("Moles")]
		public void TestGetFrob()
		{
			var rspRaw = @"<rsp stat=""ok""><frob>frob</frob></rsp>";

			MRtmAuthorizer.AllInstances.GetRtmResponseString = (a, method) =>
			{
				method.Is("rtm.auth.getFrob");
				return Observable.Return(rspRaw);
			};

			IObservable<string> result = rtm.GetFrob();
			string frob = result.First();

			frob.Is("frob");
		}

		[TestMethod]
		public void TestCreateRtmWebRequest()
		{
			WebRequest request = RtmAuthorizer.CreateRtmWebRequest();

			request.RequestUri.OriginalString.Is("https://api.rememberthemilk.com/services/rest/");
			request.Method.Is("POST");
			request.ContentType.Is("application/x-www-form-urlencoded");
		}
	}
}
