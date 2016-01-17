using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace JUnitTest 
{
[TestFixture]
[Category("Blackjack Tests")]
public class MyUnityTest1
{
	[Test]
	[Category("Rule Tests")]
	public void RuleTest()
	{
		Assert.Pass();
	}


}

}