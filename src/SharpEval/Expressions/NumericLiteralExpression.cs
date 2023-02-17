using System.Globalization;

namespace SharpEval.Expressions;

public sealed class NumericLiteralExpression : Expression
{
	public double Value { get; }
	
	public NumericLiteralExpression(double value)
	{
		Value = value;
	}
	
	public override string ToString()
	{
		return Value.ToString(CultureInfo.InvariantCulture);
	}
	
	public override string ToDebugString()
	{
		return $"{base.ToDebugString()}({Value})";
	}
}