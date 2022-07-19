using SharpEval.Tokens;
using SharpEval.Tokens.Expressions;
using SharpEval.Tokens.Literals;
using SharpEval.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpEval;

public sealed class Interpreter
{
	public Dictionary<string, IVariable> Variables { get; }

	public Dictionary<string, Func<double[], double>> Functions { get; }

	public Interpreter(Dictionary<string, IVariable>? variables = null, Dictionary<string, Func<double[], double>>? functions = null)
	{
		Variables = variables ?? new Dictionary<string, IVariable>();

		Functions = functions ?? new Dictionary<string, Func<double[], double>>();
	}

	private double EvaluateToken(IToken token)
	{
		switch (token)
		{
			case DoubleLiteral doubleLiteral:
			{
				return doubleLiteral.Value;
			}

			case Identifier identifier:
			{
				return Variables.TryGetValue(identifier.Value, out IVariable variable) ? variable.Value : throw new Exception($"Undefined constant '{identifier.Value}'.");
			}

			case ParenthesizedExpression parenthesizedExpression:
			{
				return EvaluateExpression(parenthesizedExpression.Tokens);
			}

			case InvocationExpression invocationExpression:
			{
				double[] arguments = invocationExpression.Arguments.Select(EvaluateToken).ToArray();

				return Functions.TryGetValue(invocationExpression.Identifier.Value, out Func<double[], double>? function)
					? function(arguments)
					: throw new Exception($"Undefined function '{invocationExpression.Identifier.Value}'.");
			}

			case Argument argument:
			{
				return EvaluateExpression(argument.Tokens);
			}

			default:
			{
				throw new Exception($"Unexpected token type '{token.GetType().Name}'.");
			}
		}
	}

	public double EvaluateExpression(IToken[] tokens)
	{
		if (tokens.Length == 0) return 0.0d;

		Stack<double> operandStack = new();

		foreach (IToken token in InfixToPostfix(tokens))
		{
			if (token is Operator @operator)
			{
				double operand1;
				double operand2;

				if (@operator == Operator.Plus)
				{
					operand2 = operandStack.Pop();
					operand1 = operandStack.Pop();

					operandStack.Push(operand1 + operand2);
				}
				else if (@operator == Operator.Minus)
				{
					operand2 = operandStack.Pop();
					operand1 = operandStack.Pop();

					operandStack.Push(operand1 - operand2);
				}
				else if (@operator == Operator.Times)
				{
					operand2 = operandStack.Pop();
					operand1 = operandStack.Pop();

					operandStack.Push(operand1 * operand2);
				}
				else if (@operator == Operator.Divide)
				{
					operand2 = operandStack.Pop();
					operand1 = operandStack.Pop();

					operandStack.Push(operand1 / operand2);
				}
				else if (@operator == Operator.UnaryPlus)
				{
					operand1 = operandStack.Pop();

					operandStack.Push(+operand1);
				}
				else if (@operator == Operator.UnaryMinus)
				{
					operand1 = operandStack.Pop();

					operandStack.Push(-operand1);
				}
			}
			else
			{
				operandStack.Push(EvaluateToken(token));
			}
		}

		if (operandStack.Count > 1) throw new Exception("Invalid expression.");

		return operandStack.Pop();
	}

	private static int PrecedenceOf(IToken token)
	{
		return token switch
		{
			Operator @operator when @operator == Operator.Plus || @operator == Operator.Minus => 0,

			Operator @operator when @operator == Operator.Times || @operator == Operator.Divide => 1,

			Operator @operator when @operator == Operator.UnaryPlus || @operator == Operator.UnaryMinus => 2,

			IExpression => 3,

			_ => throw new ArgumentException($"Cannot calculate precedence of a '{token.GetType().Name}' token.", nameof(token)),
		};
	}

	private static IEnumerable<IToken> InfixToPostfix(ICollection<IToken> tokens)
	{
		if (tokens.Count == 0) return new IToken[0];

		List<IToken> output = new();

		Stack<IToken> operandStack = new();

		foreach (IToken token in tokens)
		{
			switch (token)
			{
				case ILiteral or Identifier or InvocationExpression:
				{
					output.Add(token);

					break;
				}

				case Operator { IsUnary: true } when operandStack.Count > 0 && operandStack.Peek() is Operator { IsUnary: true }:
				{
					operandStack.Push(token);

					break;
				}

				default:
				{
					while (operandStack.Count > 0 && PrecedenceOf(token) <= PrecedenceOf(operandStack.Peek()))
					{
						output.Add(operandStack.Pop());
					}

					operandStack.Push(token);

					break;
				}
			}
		}

		while (operandStack.Count > 0)
		{
			output.Add(operandStack.Pop());
		}

		return output;
	}
}
