using System;

namespace SharpEval.Tokens.Expressions;

public sealed class InvocationExpression : IToken, IExpression
{
	public Identifier Identifier { get; }
	
	public Argument[] Arguments { get; }
	
	public InvocationExpression(Identifier identifier, Argument[] arguments)
	{
		Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
		
		Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
	}
}
