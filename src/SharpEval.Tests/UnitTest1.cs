using SharpEval.Expressions;
using SharpEval.Tokens;
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
		Interpreter.DictionaryContext context = new()
		{
			Values =
			{
				["x"] = 10.0d,
				["y"] = 20.0d,
			},

			Functions =
			{
				["sum"] = args => args.Sum(),
			},
		};

		Token[] tokens = new Tokenizer(" sum ( x , sum ( y , sum ( 1 , 2 ) ) ) ").ToArray();

		Expression expression = new Parser(tokens).ParseExpression();

		double result = Interpreter.Evaluate(expression, context);

		_testOutputHelper.WriteLine(result.ToString(CultureInfo.InvariantCulture)); // 33
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
