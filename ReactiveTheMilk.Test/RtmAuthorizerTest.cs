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

		[TestMethod]
		[HostType("Moles")]
		public void TestGetRtmResponse()
		{
			MRtmBase.AllInstances.GenerateSignatureIEnumerableOfParameter =
				(r, parameters) =>
				{
					var array = parameters.ToArray();
					array[0].Key.Is("method");
					array[0].Value.Is("method");

					array[1].Key.Is("api_key");
					array[1].Value.Is(ApiKey);

					return "signature";
				};

			MParametersExtension.ToPostParameterIEnumerableOfParameter =
				parameters =>
				{
					var array = parameters.ToArray();
					array[0].Key.Is("method");
					array[0].Value.Is("method");

					array[1].Key.Is("api_key");
					array[1].Value.Is(ApiKey);

					array[2].Key.Is("api_sig");
					array[2].Value.Is("signature");

					return "parameters";
				};

			var request = new SWebRequest();
			MRtmAuthorizer.CreateRtmWebRequest = () => request;

			var response = new SWebResponse();
			MWebRequestExtensions.UploadStringAsyncWebRequestStringEncoding =
				(req, param, encoding) =>
				{
					req.Is(request);
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

			string rspRaw = rtm.GetRtmResponse("method").First();

			rspRaw.Is(@"<rsp stat=""ok""></rsp>");
		}

		[TestMethod]
		[HostType("Moles")]
		public void TestGetRtmResponseWithParameters()
		{
			MRtmBase.AllInstances.GenerateSignatureIEnumerableOfParameter =
				(r, parameters) =>
				{
					var array = parameters.ToArray();
					array[0].Key.Is("key");
					array[0].Value.Is("value");

					array[1].Key.Is("method");
					array[1].Value.Is("method");

					array[2].Key.Is("api_key");
					array[2].Value.Is(ApiKey);

					return "signature";
				};

			MParametersExtension.ToPostParameterIEnumerableOfParameter =
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

			var request = new SWebRequest();
			MRtmAuthorizer.CreateRtmWebRequest = () => request;

			var response = new SWebResponse();
			MWebRequestExtensions.UploadStringAsyncWebRequestStringEncoding =
				(req, param, encoding) =>
				{
					req.Is(request);
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

			string rspRaw = rtm.GetRtmResponse("method", new[] {new Parameter("key", "value")}).First();

			rspRaw.Is(@"<rsp stat=""ok""></rsp>");
		}
	}
}
