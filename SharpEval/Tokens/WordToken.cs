namespace SharpEval.Tokens
{
	public sealed class WordToken : IToken
	{
		public IToken[] Tokens { get; }

		public WordToken(IToken[] tokens) => Tokens = tokens;

		public override string ToString() => string.Concat(Tokens);
	}
}
