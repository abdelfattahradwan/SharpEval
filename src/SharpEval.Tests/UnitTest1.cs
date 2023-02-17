using SharpEval.Expressions;
using SharpEval.Tokens;
using System.Diagnostics;
using System.Globalization;
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
		const string input = "1+4-4*4+4/4/4-4/4+4*4+4+4-4+4/4-4-4+4/4/4+4*4+4+4-4/4*4*4+4-4+4/4+4*4+4+4*4-4-4/4+4/4+4+4/4*4-4+4/4+4*4-4/4*4+4+4-4+4/4*4*4-4/4+44-4+4/4/4*4-4+4*4";
		
		Tokenizer tokenizer = new(input);
		
		Token[] tokens = tokenizer.ToArray();

		foreach (Token token in tokens)
		{
			_testOutputHelper.WriteLine(token.ToDebugString());
		}
		
		Parser parser = new(tokens);

		Expression expression = parser.ParseExpression();
		
		double result = Interpreter.Evaluate(expression, new TestContext());
		
		_testOutputHelper.WriteLine(result.ToString(CultureInfo.InvariantCulture));
	}
}

file class TestContext : Interpreter.IContext
{
	public double GetValue(string name)
	{
		return name switch
		{
			"pi" => Math.PI,
			"e" => Math.E,
		
			_ => throw new Exception($"Unknown identifier: {name}"),
		};
	}

	public Func<double[], double> GetFunction(string name)
	{
		return name switch
		{
			"sin" => args => Math.Sin(args[0]),
			"cos" => args => Math.Cos(args[0]),
			"tan" => args => Math.Tan(args[0]),
			"sqrt" => args => Math.Sqrt(args[0]),
			"pow" => args => Math.Pow(args[0], args[1]),
		
			_ => throw new Exception($"Unknown function: {name}"),
		};
	}
}
