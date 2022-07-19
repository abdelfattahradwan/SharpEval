namespace SharpEval.Variables;

public sealed class ReadOnlyVariable : IVariable
{
	public double Value { get; }

	public ReadOnlyVariable(double value)
	{
		Value = value;
	}
}