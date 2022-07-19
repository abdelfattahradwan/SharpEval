using SharpEval.Utilities;
using System;

namespace SharpEval.Tokens;

public sealed class Identifier : IToken
{
	public string Value { get; }
	
	public Identifier(string value)
	{
		Value = StringUtilities.IsNullOrWhiteSpace(value) ? throw new ArgumentException(null, nameof(value)) : value;
	}
}
