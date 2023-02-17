namespace SharpEval.Expressions;

public sealed class UnaryExpression : Expression
{
	public Operator Operator { get; }
	
	public Expression Operand { get; }
	
	public UnaryExpression(Operator @operator, Expression operand)
	{
		Operator = @operator;
		
		Operand = operand;
	}
	
	public override string ToString()
	{
		return $"{Operator.GetSymbol()}{Operand}";
	}
	
	public override string ToDebugString()
	{
		return $"{base.ToDebugString()}({Operator.GetSymbol()}, {Operand.ToDebugString()})";
	}
}