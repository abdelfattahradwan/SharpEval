using System.Collections.Generic;

namespace SharpEval.Tokens
{
	public sealed class WordToken : IToken
	{
		public Queue<IToken> Symbols { get; }

		public WordToken(Queue<IToken> symbols) => Symbols = symbols;

		public override string ToString() => string.Concat(Symbols);
	}
}
