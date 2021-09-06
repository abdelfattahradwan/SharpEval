using System;

namespace SharpEval.Variables
{
	public sealed class ComputedVariable : IVariable
	{
		private readonly Func<double> _computeValue;

		public double Value => _computeValue?.Invoke() ?? 0.0d;

		public ComputedVariable(Func<double> computeValue) => _computeValue = computeValue;
	}
}
