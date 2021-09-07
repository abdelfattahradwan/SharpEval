namespace SharpEval.Tokens
{
	public sealed class NumberToken : IToken
	{
		public IToken[] Tokens { get; }

		public double Value => double.Parse(ToString());

		public NumberToken(IToken[] tokens) => Tokens = tokens;

		public override string ToString() => string.Concat(Tokens);
	}
}
