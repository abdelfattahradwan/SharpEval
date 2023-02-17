namespace SharpEval.Expressions;

public sealed class IdentifierExpression : Expression
{
	public string Name { get; }

	public IdentifierExpression(string name)
	{
		Name = name;
	}

	public override string ToString()
	{
		return Name;
	}

	public override string ToDebugString()
	{
		return $"{base.ToDebugString()}({Name})";
	}
}