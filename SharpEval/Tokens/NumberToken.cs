using System.Collections.Generic;

namespace SharpEval.Tokens
{
	public sealed class NumberToken : IToken
	{
		public Queue<IToken> Symbols { get; }

		public double Value => double.Parse(ToString());

		public NumberToken(Queue<IToken> symbols) => Symbols = symbols;

		public override string ToString() => string.Concat(Symbols);
	}
}
