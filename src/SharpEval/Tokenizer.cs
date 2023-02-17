using SharpEval.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SharpEval;

public sealed class Tokenizer : IEnumerable<Token>
{
	private readonly string _input;

	private int _position;

	public Tokenizer(string input)
	{
		_input = input;

		_position = 0;
	}

	private char Peek()
	{
		return _position < _input.Length ? _input[_position] : '\0';
	}

	private char Read()
	{
		return _position < _input.Length ? _input[_position++] : '\0';
	}

	private Token? ReadToken()
	{
		while (true)
		{
			switch (Peek())
			{
				case '+':
				{
					Read();

					return new OperatorToken(Operator.Plus);
				}

				case '-':
				{
					Read();

					return new OperatorToken(Operator.Minus);
				}

				case '*':
				{
					Read();

					return new OperatorToken(Operator.Times);
				}

				case '/':
				{
					Read();

					return new OperatorToken(Operator.Divide);
				}

				case '%':
				{
					Read();

					return new OperatorToken(Operator.Modulo);
				}

				case '(':
				{
					Read();

					return new LeftParenthesisToken();
				}

				case ')':
				{
					Read();

					return new RightParenthesisToken();
				}

				case '\0':
				{
					return null;
				}

				default:
				{
					if (char.IsWhiteSpace(Peek()))
					{
						Read();

						continue;
					}

					if (char.IsDigit(Peek()))
					{
						double value = 0.0D;

						while (char.IsDigit(Peek()))
						{
							value = value * 10.0D + (Read() - '0');
						}

						if (Peek() is not '.') return new NumericLiteralToken(value);

						Read();

						double fraction = 0.0D;

						double divisor = 1.0D;

						while (char.IsDigit(Peek()))
						{
							fraction = fraction * 10.0D + (Read() - '0');

							divisor *= 10.0D;
						}

						value += fraction / divisor;

						return new NumericLiteralToken(value);
					}

					if (char.IsLetter(Peek()))
					{
						string name = string.Empty;

						while (char.IsLetterOrDigit(Peek()))
						{
							name += Read();
						}

						return new IdentifierToken(name);
					}

					throw new Exception($"Unexpected character '{Peek()}'");
				}
			}
		}
	}

	public IEnumerator<Token> GetEnumerator()
	{
		while (ReadToken() is { } token)
		{
			yield return token;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
