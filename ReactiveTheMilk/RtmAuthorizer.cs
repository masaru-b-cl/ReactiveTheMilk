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
    public RtmAuthorizer(string apiKey, string secret)
      : base(apiKey, secret)
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

      string signature = this.signatureGenerator.Generate(parameters);
      parameters.Add("api_sig", signature);


      string postData = parameters.ToPostData();
      return @"http://www.rememberthemilk.com/services/auth/?" + postData;
    }

    public IObservable<RtmToken> GetToken(string frob)
    {
      return GetRtmResponse("rtm.auth.getToken", new Parameter("frob", frob))
        .Select(rspRaw => XElement.Parse(rspRaw))
        .Select(rsp =>
          {
            var auth = rsp.Element("auth");
            var user = auth.Element("user");
            return new RtmToken
              {
                Token = (string)auth.Element("token"),
                Perms = (string)auth.Element("perms"),
                User = new RtmUser
                {
                  Id = (int)user.Attribute("id"),
                  Username = (string)user.Attribute("username"),
                  Fullname = (string)user.Attribute("fullname")
                }
              };
          }
        );
    }
  }
}
