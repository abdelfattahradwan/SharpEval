namespace SharpEval.Tokens.Literals;

public sealed class CharLiteral : IToken, ILiteral
{
	public char Value { get; }

	public CharLiteral(char value)
	{
		Value = value;
	}
}