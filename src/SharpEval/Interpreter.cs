using SharpEval.Expressions;
using System;

namespace SharpEval;

public static class Interpreter
{
	public interface IContext
	{
		double GetValue(string name);

		Func<double[], double> GetFunction(string name);
	}

	public static double Evaluate(Expression expression, IContext context)
	{
		return expression switch
		{
			NumericLiteralExpression numericLiteralExpression => numericLiteralExpression.Value,

			IdentifierExpression identifierExpression => context.GetValue(identifierExpression.Name),

			CallExpression callExpression => EvaluateCallExpression(callExpression, context),

			UnaryExpression unaryExpression => EvaluateUnaryExpression(unaryExpression, context),

			BinaryExpression binaryExpression => EvaluateBinaryExpression(binaryExpression, context),

			_ => throw new Exception($"Unexpected expression: {expression}"),
		};
	}

	private static double EvaluateCallExpression(CallExpression callExpression, IContext context)
	{
		Func<double[], double> function = context.GetFunction(callExpression.Target.ToString());

		double[] arguments = new double[callExpression.Arguments.Length];

		for (int i = 0; i < arguments.Length; i++)
		{
			arguments[i] = Evaluate(callExpression.Arguments[i], context);
		}

		return function(arguments);
	}

	private static double EvaluateUnaryExpression(UnaryExpression unaryExpression, IContext context)
	{
		double operand = Evaluate(unaryExpression.Operand, context);

		return unaryExpression.Operator switch
		{
			Operator.Plus => operand,

			Operator.Minus => -operand,

			_ => throw new Exception($"Unexpected operator: {unaryExpression.Operator}"),
		};
	}

	private static double EvaluateBinaryExpression(BinaryExpression binaryExpression, IContext context)
	{
		double left = Evaluate(binaryExpression.Left, context);

		double right = Evaluate(binaryExpression.Right, context);

		return binaryExpression.Operator switch
		{
			Operator.Plus => left + right,

			Operator.Minus => left - right,

			Operator.Times => left * right,

			Operator.Divide => left / right,

			Operator.Modulo => left % right,

			_ => throw new Exception($"Unexpected operator: {binaryExpression.Operator}"),
		};
	}
}
