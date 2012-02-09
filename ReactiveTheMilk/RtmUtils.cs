using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ReactiveTheMilk
{
  public static class RtmUtils
  {
    /// <summary>
    /// signature生成
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static string GenerateSignature(IEnumerable<Parameter> parameters, string secret)
    {
      // パラメータをキー順に並べ、キーと値を並べて、全パラメータ文連結する
      var paramstr = parameters
        .OrderBy(x => x.Key)
        .Select(x => x.Key + x.Value)
        .Concat();

      // Secretと連結し、MD5ハッシュを出す
      string source = secret + paramstr;
      using (var md5 = MD5.Create())
      {
        byte[] md5sumBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
        return BitConverter.ToString(md5sumBytes).ToLower().Replace("-", "");
      }
    }
  }
}
