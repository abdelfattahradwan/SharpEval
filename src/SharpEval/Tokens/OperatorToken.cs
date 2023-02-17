namespace SharpEval.Tokens;

public sealed class OperatorToken : Token
{
	public Operator Operator { get; }

	public OperatorToken(Operator @operator)
	{
		Operator = @operator;
	}

	public override string ToString()
	{
		return Operator.GetSymbol();
	}

	public override string ToDebugString()
	{
		return $"{base.ToDebugString()}({Operator})";
	}
}
