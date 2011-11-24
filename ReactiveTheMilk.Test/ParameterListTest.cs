using ReactiveTheMilk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ReactiveTheMilk
{
	[TestClass]
	public class ParameterTest
	{
		[TestMethod]
		public void TestAdd()
		{
			// 前準備
			var parameters = new ParameterList();

			// 実行
			parameters.Add("key", "value");

			// 検証
			Assert.AreEqual(parameters.Count, 1);
		}

		[TestMethod]
		public void TestToPostParameter()
		{
			// 前準備
			var parameters = new ParameterList() {
				new Parameter("key1", "value1"),
				new Parameter("key 2", "value 2")
			};

			Assert.AreEqual(parameters.ToPostParameter(), "key1=value1&key+2=value+2");
		}
	}
}
