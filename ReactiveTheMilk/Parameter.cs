using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ReactiveTheMilk
{
	/// <summary>
	/// RTM APIパラメーター
	/// </summary>
	public class Parameter
	{
		public string Key { get; private set; }
		public string Value { get; set; }

		public Parameter(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}
	}

	public static class ParametersExtension
	{
		public static void Add(this ICollection<Parameter> parameters, string key, string value)
		{
			parameters.Add(new Parameter(key, value));
		}

		/// <summary>
		/// RTM APIパラメーターをPOSTデータ用の文字列にする
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static String ToPostData(this IEnumerable<Parameter> parameters)
		{
			return parameters
				.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value))
				.Join("&");
		}
	}
}
