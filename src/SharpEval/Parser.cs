using SharpEval.Expressions;
using SharpEval.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEval;

public sealed class Parser
{
	private const int AdditiveExpressionPrecedence = 1;
	private const int MultiplicativeExpressionPrecedence = 2;
	private const int UnaryExpressionPrecedence = 3;
	private const int CallExpressionPrecedence = 4;
	private const int PrimaryExpressionPrecedence = 5;

	private readonly Token[] _tokens;

	private int _position;

	private readonly SortedList<int, Func<Expression>> _expressionParsersByPrecedence;

	public Parser(Token[] tokens)
	{
		_tokens = tokens;

		_position = 0;

		_expressionParsersByPrecedence = new SortedList<int, Func<Expression>>
		{
			{ AdditiveExpressionPrecedence, ParseAdditiveExpression },
			{ MultiplicativeExpressionPrecedence, ParseMultiplicativeExpression },
			{ UnaryExpressionPrecedence, ParseUnaryExpression },
			{ CallExpressionPrecedence, ParseCallExpression },
			{ PrimaryExpressionPrecedence, ParsePrimaryExpression },
		};
	}

	private Token? Peek()
	{
		return _position < _tokens.Length ? _tokens[_position] : null;
	}

	private Token? Read()
	{
		return _position < _tokens.Length ? _tokens[_position++] : null;
	}
	
	public Expression ParseExpression(int precedence = AdditiveExpressionPrecedence)
	{
		if (precedence is > PrimaryExpressionPrecedence or < AdditiveExpressionPrecedence) throw new Exception($"Invalid precedence: {precedence}");

		return _expressionParsersByPrecedence[precedence]();
	}

	private Expression ParseAdditiveExpression()
	{
		Expression left = ParseExpression(MultiplicativeExpressionPrecedence);

		while (true)
		{
			switch (Peek())
			{
				case OperatorToken { Operator: Operator.Plus or Operator.Minus } operatorToken:
				{
					Read();

					Expression right = ParseExpression(MultiplicativeExpressionPrecedence);

					left = new BinaryExpression(left, operatorToken.Operator, right);

					break;
				}

				default:
				{
					return left;
				}
			}
		}
	}

	private Expression ParseMultiplicativeExpression()
	{
		Expression left = ParseExpression(UnaryExpressionPrecedence);

		while (true)
		{
			switch (Peek())
			{
				case OperatorToken { Operator: Operator.Times or Operator.Divide or Operator.Modulo } operatorToken:
				{
					Read();

					Expression right = ParseExpression(UnaryExpressionPrecedence);

					left = new BinaryExpression(left, operatorToken.Operator, right);

					break;
				}

				default:
				{
					return left;
				}
			}
		}
	}

	private Expression ParseUnaryExpression()
	{
		switch (Peek())
		{
			case OperatorToken { Operator: Operator.Plus or Operator.Minus } operatorToken:
			{
				Read();

				Expression operand = ParseExpression();

				return new UnaryExpression(operatorToken.Operator, operand);
			}

			default:
			{
				return ParseExpression(CallExpressionPrecedence);
			}
		}
	}
	
	private Expression ParseCallExpression()
	{
		Expression expression = ParseExpression(PrimaryExpressionPrecedence);

		while (true)
		{
			switch (Peek())
			{
				case LeftParenthesisToken:
				{
					Read();

					List<Expression> arguments = new();

					while (true)
					{
						switch (Peek())
						{
							case RightParenthesisToken:
							{
								Read();

								return new CallExpression(expression, arguments.ToArray());
							}

							default:
							{
								arguments.Add(ParseExpression());

								if (Peek() is CommaToken)
								{
									Read();
								}

								break;
							}
						}
					}
				}

				default:
				{
					return expression;
				}
			}
		}
	}

	private Expression ParsePrimaryExpression()
	{
		switch (Read())
		{
			case NumericLiteralToken numericLiteralToken:
			{
				return new NumericLiteralExpression(numericLiteralToken.Value);
			}
			
			case IdentifierToken identifierToken:
			{
				return new IdentifierExpression(identifierToken.Name);
			}

			case LeftParenthesisToken:
			{
				Expression expression = ParseExpression();

				if (Read() is not RightParenthesisToken) throw new Exception("Expected right parenthesis");

				return expression;
			}

			default:
			{
				throw new Exception($"Unexpected token: {Peek()}");
			}
		}
	}
}
