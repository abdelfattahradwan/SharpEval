using System.Globalization;

namespace SharpEval.Tokens;

public sealed class NumericLiteralToken : Token
{
	public double Value { get; }
	
	public NumericLiteralToken(double value)
	{
		Value = value;
	}
	
	public override string ToString()
	{
		return Value.ToString(CultureInfo.InvariantCulture);
	}
	
	public override string ToDebugString()
	{
		return $"{base.ToDebugString()}({Value.ToString(CultureInfo.InvariantCulture)})";
	}
}