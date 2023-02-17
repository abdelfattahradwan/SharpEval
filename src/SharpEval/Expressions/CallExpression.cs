using System.Linq;
using System.Text;

namespace SharpEval.Expressions;

public sealed class CallExpression : Expression
{
	public Expression Target { get; }

	public Expression[] Arguments { get; }

	public CallExpression(Expression target, Expression[] arguments)
	{
		Target = target;

		Arguments = arguments;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new();

		stringBuilder.Append(Target);

		stringBuilder.Append('(');

		for (int i = 0; i < Arguments.Length; i++)
		{
			stringBuilder.Append(Arguments[i]);

			if (i < Arguments.Length - 1) stringBuilder.Append(',').Append(' ');
		}

		stringBuilder.Append(')');

		return stringBuilder.ToString();
	}

	public override string ToDebugString()
	{
		StringBuilder stringBuilder = new();

		stringBuilder.Append(base.ToDebugString());

		stringBuilder.Append('(');

		stringBuilder.Append(Target.ToDebugString());

		stringBuilder.Append(", ");

		stringBuilder.Append('[');

		for (int i = 0; i < Arguments.Length; i++)
		{
			stringBuilder.Append(Arguments[i].ToDebugString());

			if (i < Arguments.Length - 1) stringBuilder.Append(',').Append(' ');
		}

		stringBuilder.Append(']');

		stringBuilder.Append(')');

		return stringBuilder.ToString();
	}
}
