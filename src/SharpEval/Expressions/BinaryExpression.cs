namespace SharpEval.Expressions;

public sealed class BinaryExpression : Expression
{
	public Expression Left { get; }
	
	public Operator Operator { get; }
	
	public Expression Right { get; }
	
	public BinaryExpression(Expression left, Operator @operator, Expression right)
	{
		Left = left;
		
		Operator = @operator;
	
		Right = right;
	}
	
	public override string ToString()
	{
		return $"{Left} {Operator.GetSymbol()} {Right}";
	}
	
	public override string ToDebugString()
	{
		return $"{base.ToDebugString()}({Left.ToDebugString()}, {Operator.GetSymbol()}, {Right.ToDebugString()})";
	}
}
