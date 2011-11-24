using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ReactiveTheMilk
{
  /// <summary>
  /// RTM APIの共通的な処理を行う
  /// </summary>
  public class RtmBase
  {
    /// <summary>
    /// RTM API Key
    /// </summary>
    protected string _apiKey;

    /// <summary>
    /// RTM API Secret
    /// </summary>
    protected string _secret;

    public RtmBase(string apiKey, string secret)
    {
      this._apiKey = apiKey;
      this._secret = secret;
    }

    public string GenerateSignature(IEnumerable<Parameter> parameters)
    {
      // パラメータをキー順に並べ、キーと値を並べて、全パラメータ文連結する
      var paramstr = parameters
        .OrderBy(x => x.Key)
        .Select(x => x.Key + x.Value)
        .Concat();

      // Secretと連結し、MD5ハッシュを出す
      string source = this._secret + paramstr;
      using (var md5 = MD5.Create())
      {
        byte[] md5sumBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
        return BitConverter.ToString(md5sumBytes).ToLower().Replace("-", "");
      }
    }

  }
}
