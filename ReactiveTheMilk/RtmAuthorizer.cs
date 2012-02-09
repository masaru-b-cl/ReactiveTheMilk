using System;
using System.Reactive.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

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

		/// <summary>
		/// frob取得
		/// </summary>
		/// <returns></returns>
		public IObservable<string> GetFrob()
		{
			return GetRtmResponse("rtm.auth.getFrob")
				.Select(rspRaw => XElement.Parse(rspRaw))
				.Select(rsp => rsp.Element("frob").Value);
		}


    public string GetAuthenticationUrl(string frob)
    {
      var parameters = new List<Parameter>();
      parameters.Add("api_key", this._apiKey);
      parameters.Add("perms", "delete");
      parameters.Add("frob", frob);

      string signature = GenerateSignature(parameters);
      parameters.Add("api_sig", signature);


      string postData = parameters.ToPostData();
      return @"http://www.rememberthemilk.com/services/auth/?" + postData;
    }
  }
}
