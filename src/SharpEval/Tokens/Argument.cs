namespace SharpEval.Tokens;

public sealed class Argument : IToken
{
	public IToken[] Tokens { get; }
	
	public Argument(IToken[] tokens)
	{
		Tokens = tokens;
	}
}
