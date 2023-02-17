namespace SharpEval.Tokens;

public abstract class Token
{
	public override abstract string ToString();

	public virtual string ToDebugString()
	{
		return GetType().Name;
	}
}
