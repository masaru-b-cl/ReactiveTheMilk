using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveTheMilk
{
  public class RtmToken
  {
    public string Token { get; set; }
    public string Perms { get; set; }
    public RtmUser User { get; set; }
  }
}
