using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ReactiveTheMilk
{
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

	public class ParameterList : List<Parameter>
	{
		public void Add(string key, string value)
		{
			Add(new Parameter(key, value));
		}
	}

	public static class ParametersExtension
	{
		public static String ToPostParameter(this IEnumerable<Parameter> parameters)
		{
			return parameters
				.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value))
				.Join("&");
		}

		public static ParameterList ToList(this IEnumerable<Parameter> parameters)
		{
			var list = new ParameterList();
			foreach (var item in parameters)
			{
				list.Add(item.Key, item.Value);
			}
			return list;
		}
	}
}
