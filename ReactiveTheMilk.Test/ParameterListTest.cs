using ReactiveTheMilk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ReactiveTheMilk
{
	[TestClass]
	public class ParameterTest
	{
		[TestMethod]
		public void TestAdd()
		{
			// 前準備
			var parameters = new List<Parameter>();

			// 実行
			parameters.Add("key", "value");

			// 検証
			Assert.AreEqual(parameters.Count, 1);
		}

		[TestMethod]
		public void TestToPostData()
		{
			// 前準備
			var parameters = new List<Parameter>() {
				new Parameter("key1", "value1"),
				new Parameter("key 2", "value 2")
			};

			Assert.AreEqual(parameters.ToPostData(), "key1=value1&key+2=value+2");
		}
	}
}
