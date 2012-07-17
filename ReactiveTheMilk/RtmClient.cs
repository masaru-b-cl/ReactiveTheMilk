using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Xml.Linq;

namespace ReactiveTheMilk
{
  public class RtmClient : RtmBase
  {
    internal string Token { get; private set; }

    public RtmClient(string apiKey, string secret, string token)
      : base(apiKey, secret)
    {
      this.Token = token;
    }

    /// <summary>
    /// Retrieves a list of lists.
    /// </summary>
    /// <returns></returns>
    public IObservable<RtmList> GetLists()
    {
      return GetRtmResponse("rtm.lists.getList")
        .Select(rspRaw => XElement.Parse(rspRaw))
        .Select(rsp => rsp.Descendants("list"))
        .SelectMany(lists => lists)
        .Select(list => new RtmList 
          {
            Id = (string)list.Attribute("id"),
            Name = (string)list.Attribute("name"),
            Deleted = ((string)list.Attribute("deleted") == "1"),
            Locked = ((string)list.Attribute("locked") == "1"),
            Archived = ((string)list.Attribute("archived") == "1"),
            Position = (int)list.Attribute("position"),
            Smart = ((string)list.Attribute("smart") == "1"),
            Filter = list.Element("filter").Value,
          })
        ;
    }

    /// <summary>
    /// method、パラメーターを指定してRTM APIレスポンス取得
    /// </summary>
    /// <param name="method">method</param>
    /// <param name="parameters">パラメーター</param>
    /// <returns>RTM APIレスポンスの生XML</returns>
    internal new IObservable<string> GetRtmResponse(string method, params Parameter[] parameters)
    {
      var paramList = parameters.ToList();
      paramList.Add("auth_token", this.Token);
      return base.GetRtmResponse("rtm.lists.getList", paramList.ToArray());
    }
  }
}
