namespace SharpEval.Expressions;

public abstract class Expression
{
	public override abstract string ToString();
	
	public virtual string ToDebugString()
	{
		return GetType().Name;
	}
}
