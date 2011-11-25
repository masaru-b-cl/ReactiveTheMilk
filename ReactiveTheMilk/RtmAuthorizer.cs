using System;
using System.Reactive.Linq;
using System.Xml.Linq;

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

	}
}
