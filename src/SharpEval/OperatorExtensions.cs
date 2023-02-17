using System;

namespace SharpEval;

public static class OperatorExtensions
{
	public static bool IsUnary(this Operator @operator)
	{
		return @operator is Operator.UnaryPlus or Operator.UnaryMinus;
	}

	public static bool IsBinary(this Operator @operator)
	{
		return @operator is Operator.Plus or Operator.Minus or Operator.Times or Operator.Divide or Operator.Modulo;
	}

	public static bool IsAdditive(this Operator @operator)
	{
		return @operator is Operator.Plus or Operator.Minus;
	}

	public static bool IsMultiplicative(this Operator @operator)
	{
		return @operator is Operator.Times or Operator.Divide or Operator.Modulo;
	}

	public static bool IsLeftAssociative(this Operator @operator)
	{
		return @operator is Operator.Plus or Operator.Minus or Operator.Times or Operator.Divide or Operator.Modulo;
	}

	public static bool IsRightAssociative(this Operator @operator)
	{
		return @operator is Operator.UnaryPlus or Operator.UnaryMinus;
	}

	public static int GetPrecedence(this Operator @operator)
	{
		return @operator switch
		{
			Operator.Plus or Operator.Minus => 1,

			Operator.Times or Operator.Divide or Operator.Modulo => 2,

			Operator.UnaryPlus or Operator.UnaryMinus => 3,

			_ => throw new Exception("Unreachable"),
		};
	}
	
	public static string GetSymbol(this Operator @operator)
	{
		return @operator switch
		{
			Operator.Plus => "+",
			Operator.Minus => "-",
			
			Operator.Times => "*",
			Operator.Divide => "/",
			Operator.Modulo => "%",
			
			Operator.UnaryPlus => "+",
			Operator.UnaryMinus => "-",
			
			_ => throw new Exception("Unreachable"),
		};
	}
}
