using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
  }
}
