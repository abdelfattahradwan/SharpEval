namespace SharpEval.Tokens.Literals;

public sealed class DoubleLiteral : IToken, ILiteral
{
	public double Value { get; }
	
	public DoubleLiteral(double value)
	{
		Value = value;
	}
}
