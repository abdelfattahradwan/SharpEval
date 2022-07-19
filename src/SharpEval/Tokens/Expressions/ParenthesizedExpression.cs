using System;

namespace SharpEval.Tokens.Expressions;

public sealed class ParenthesizedExpression : IToken, IExpression
{
	public IToken[] Tokens { get; }

	public ParenthesizedExpression(IToken[] tokens)
	{
		Tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
	}
}
