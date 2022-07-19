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

	private const string MathExpression =
		"( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) ) ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) ) ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) ) ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) ) ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) ) ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) ) ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) ) ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) + ( ( 2 + 2 ) / 2 - 8 * 4 + 16 ) ) )";

	[Fact]
	public void Test1()
	{
		IToken[] tokens = Tokenizer.Tokenize(MathExpression);
		
		foreach (IToken token in tokens)
		{
			_testOutputHelper.WriteLine(token.ToString());
		}

		long min = long.MaxValue;
		long max = long.MinValue;

		Stopwatch stopwatch = new();

		Interpreter interpreter = new();

		double result = 0.0d;

		for (int i = 0; i < 4_000; i++)
		{
			stopwatch.Restart();

			result = interpreter.EvaluateExpression(tokens);

			stopwatch.Stop();

			if (stopwatch.ElapsedMilliseconds < min) min = stopwatch.ElapsedMilliseconds;

			if (stopwatch.ElapsedMilliseconds > max) max = stopwatch.ElapsedMilliseconds;
		}

		_testOutputHelper.WriteLine($"Result: {result}, Took: {min}ms - {max}ms");
	}
}
