using System;

namespace SharpEval.Variables;

public sealed class ComputedVariable : IVariable
{
	private readonly Func<double> _computeValue;

	public double Value
	{
		get => _computeValue();
	}

	public ComputedVariable(Func<double> computeValue)
	{
		_computeValue = computeValue;
	}
}