namespace SharpEval.Tokens
{
	public sealed class ParameterArrayToken : IToken
	{
		public ExpressionToken[] Elements { get; }

		public ParameterArrayToken(ExpressionToken[] elements) => Elements = elements;
	}
}
