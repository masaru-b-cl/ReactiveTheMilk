using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Net;
using System.Xml.Linq;
using Codeplex.Reactive.Asynchronous;

namespace ReactiveTheMilk
{
	/// <summary>
	/// RTM API の認証に関する処理を行う
	/// </summary>
	public class RtmAuthorizer : RtmBase
	{
		public RtmAuthorizer(string apiKey, string secret) : base(apiKey, secret)
		{
		}

		public IObservable<string> GetFrob()
		{
			IObservable<string> result = GetRtmResponse("rtm.auth.getFrob");
			return result
				.Select(rspRaw => XElement.Parse(rspRaw))
				.Select(rsp => rsp.Element("frob").Value);
		}

		public IObservable<string> GetRtmResponse(string method)
		{
			return GetRtmResponse(method, new Parameter[] { });
		}

		public IObservable<string> GetRtmResponse(string method, IEnumerable<Parameter> parameters)
		{
			// パラメータリスト作成
			var paramList = parameters.ToList();
			paramList.Add("method", method);
			paramList.Add("api_key", this._apiKey);

			// signature生成
			string signature = GenerateSignature(paramList);
			paramList.Add("api_sig", signature);

			// POSTパラメータ構築
			string postParameter = paramList.ToPostParameter();

			// RTM API呼び出し
			return CreateRtmWebRequest()
				.UploadStringAsync(postParameter, Encoding.UTF8)
				.SelectMany(r => r.DownloadStringAsync(Encoding.UTF8));
		}

		public static WebRequest CreateRtmWebRequest()
		{
			WebRequest request = WebRequest.Create("https://api.rememberthemilk.com/services/rest/");
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			return request;
		}
	}
}
