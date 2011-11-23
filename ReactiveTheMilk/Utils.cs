using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveTheMilk
{
  public static class Utils
  {
    public static string Join(this IEnumerable<string> values, string separater)
    {
      return String.Join(separater, values);
    }

    public static string Concat(this IEnumerable<string> values)
    {
      return String.Concat(values);
    }
  }
}
