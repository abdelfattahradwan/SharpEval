using System.Collections.Generic;

namespace SharpEval.Tokens
{
	public sealed class ExpressionToken : IToken
	{
		public Queue<IToken> Tokens { get; }

		public ExpressionToken(Queue<IToken> tokens) => Tokens = tokens;
	}
}
