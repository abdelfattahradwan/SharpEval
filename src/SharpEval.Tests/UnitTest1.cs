using SharpEval.Tokens;
using Xunit.Abstractions;

namespace SharpEval.Tests;

public class UnitTest1
{
	private readonly ITestOutputHelper _testOutputHelper;

	public UnitTest1(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	private const string MathExpression = "-(2 + 2) - 4 * 8 / 4";

	[Fact]
	public void Test1()
	{
		IToken[] tokens = Tokenizer.Tokenize(MathExpression);

		foreach (IToken token in tokens)
		{
			_testOutputHelper.WriteLine(token.ToString());
		}

		Assert.Equal(new Interpreter().EvaluateExpression(tokens), -(2 + 2) - 4 * 8 / 4);
	}
}
