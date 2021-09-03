using System.Collections.Generic;

namespace SharpEval.Tokens
{
	public sealed class ParameterArrayToken : IToken
	{
		public Queue<ExpressionToken> Elements { get; }

		public ParameterArrayToken(Queue<ExpressionToken> elements) => Elements = elements;
	}
}
