namespace SharpEval.Tokens;

public sealed class IdentifierToken : Token
{
	public string Name { get; }
	
	public IdentifierToken(string name)
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
