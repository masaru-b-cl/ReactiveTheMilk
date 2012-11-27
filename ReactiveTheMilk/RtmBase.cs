using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Codeplex.Reactive.Asynchronous;

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

    protected readonly SignatureGenerator signatureGenerator;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="secret"></param>
    public RtmBase(string apiKey, string secret)
    {
      this._apiKey = apiKey;
      this._secret = secret;
      this.signatureGenerator = new SignatureGenerator(this._secret);
    }

    /// <summary>
    /// methodのみ指定してRTM APIレスポンス取得
    /// </summary>
    /// <param name="method">method</param>
    /// <returns>RTM APIレスポンスの生XML</returns>
    public IObservable<string> GetRtmResponse(string method)
    {
      return GetRtmResponse(method, new Parameter[] { });
    }

    /// <summary>
    /// method、パラメーターを指定してRTM APIレスポンス取得
    /// </summary>
    /// <param name="method">method</param>
    /// <param name="parameters">パラメーター</param>
    /// <returns>RTM APIレスポンスの生XML</returns>
    public IObservable<string> GetRtmResponse(string method, params Parameter[] parameters)
    {
      // パラメータリスト作成
      var paramList = parameters.ToList();
      paramList.Add("method", method);
      paramList.Add("api_key", this._apiKey);

      // signature生成
      string signature = this.signatureGenerator.Generate(paramList);
      paramList.Add("api_sig", signature);

      // POSTパラメータ構築
      string postData = paramList.ToPostData();

      // RTM API呼び出し
      return CreateRtmWebRequest()
        .UploadStringAsync(postData, Encoding.UTF8)							// POST
        .SelectMany(r => r.DownloadStringAsync(Encoding.UTF8))	// レスポンスを文字列で取得
        .Do(rspRaw =>
          {
            // レスポンスのステータス確認
            var rsp = XElement.Parse(rspRaw);
            var stat = (string)rsp.Attribute("stat");
            if (stat == "fail")
            {
              // エラー
              var err = rsp.Element("err");
              var code = (string)err.Attribute("code");
              var msg = (string)err.Attribute("msg");
              throw new RtmException(code, msg);
            }
          }
        );
    }

    /// <summary>
    /// RTM API呼び出しのためのWebRequest作成
    /// </summary>
    /// <returns>WebRequest</returns>
    private static WebRequest CreateRtmWebRequest()
    {
      WebRequest request = WebRequest.Create("https://api.rememberthemilk.com/services/rest/");
      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      return request;
    }
  }
}
