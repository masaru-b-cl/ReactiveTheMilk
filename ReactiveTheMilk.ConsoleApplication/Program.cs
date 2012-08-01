using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Diagnostics;

namespace ReactiveTheMilk.ConsoleApplication
{
  class Program
  {
    static void Main(string[] args)
    {
      string apiKey = "(api-key)";
      string secret = "(secret)";
      var rtmAuthorizer = new RtmAuthorizer(apiKey, secret);

      string frob = rtmAuthorizer.GetFrob().First();

      string authenticationUrl = rtmAuthorizer.GetAuthenticationUrl(frob);

      //Process.Start(authenticationUrl);

      //Console.WriteLine("Webページで認証してください。...");

      //Console.ReadKey();

      //var token = rtmAuthorizer.GetToken(frob).First();

      //Console.WriteLine("Token:{0}", token.Token);

      RtmClient client = new RtmClient(apiKey, secret, "(token)");

      var lists = client.GetLists();

      var list = lists.First();

    }
  }
}
