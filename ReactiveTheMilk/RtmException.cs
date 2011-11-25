using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveTheMilk
{
	/// <summary>
	/// RTM API レスポンスのエラー用例外
	/// </summary>
	public class RtmException : Exception
	{
		public string Code { get; private set; }
		public string Msg { get; private set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="code"></param>
		/// <param name="msg"></param>
		public RtmException(string code, string msg)
		{
			this.Code = code;
			this.Msg = msg;
		}
	}
}
