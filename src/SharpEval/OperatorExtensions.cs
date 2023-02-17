using System;

namespace SharpEval;

public static class OperatorExtensions
{
	public static string GetSymbol(this Operator @operator)
	{
		return @operator switch
		{
			Operator.Plus => "+",
			Operator.Minus => "-",
			
			Operator.Times => "*",
			Operator.Divide => "/",
			Operator.Modulo => "%",
			
			_ => throw new Exception("Unreachable"),
		};
	}
}
