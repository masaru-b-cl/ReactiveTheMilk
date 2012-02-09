using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveTheMilk
{
  internal static class IEnumerableExtensions
  {
		internal static string Join(this IEnumerable<string> values, string separater)
    {
      return String.Join(separater, values);
    }

		internal static string Concat(this IEnumerable<string> values)
    {
      return String.Concat(values);
    }
  }
}
