using SharpEval.Tokens;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace SharpEval.Tests;

public class UnitTest1
{
	private readonly ITestOutputHelper _testOutputHelper;

	public UnitTest1(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	public void Test1()
	{
		const string input = "11.6623156723451423641623461243651423651623416423651423661 + 2 * 3";
		
		Tokenizer tokenizer = new(input);

		foreach (Token token in tokenizer)
		{
			_testOutputHelper.WriteLine(token.ToDebugString());
		}
	}
}
