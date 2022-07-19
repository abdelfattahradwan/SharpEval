using SharpEval.Tokens;
using SharpEval.Tokens.Expressions;
using SharpEval.Tokens.Literals;
using SharpEval.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpEval;

public static class Tokenizer
{
	public static IToken[] Tokenize(string s)
	{
		if (StringUtilities.IsNullOrWhiteSpace(s)) return new IToken[0];

		List<IToken> tokens = new();

		for (int i = 0; i < s.Length; i++)
		{
			char c = s[i];

			switch (c)
			{
				case '(':
				{
					static ParenthesizedExpression ExtractParenthesizedExpression(string s, int start, out int end)
					{
						int depth = 1;

						for (int i = start; i < s.Length; i++)
						{
							switch (s[i])
							{
								case '(':
								{
									depth++;

									break;
								}

								case ')':
								{
									depth--;

									break;
								}
							}

							if (depth > 0) continue;

							end = i;

							return new ParenthesizedExpression(Tokenize(StringUtilities.Slice(s, start, i)));
						}

						throw new Exception("')' expected.");
					}

					ParenthesizedExpression parenthesizedExpression = ExtractParenthesizedExpression(s, ++i, out i);

					if (tokens.Count > 0 && tokens[tokens.Count - 1] is Identifier identifier)
					{
						tokens.RemoveAt(tokens.Count - 1);

						List<Argument> arguments = new();

						List<IToken> argumentTokens = new();

						void AddArgument()
						{
							if (argumentTokens.Count == 0) throw new Exception("Argument expected.");

							arguments.Add(new Argument(argumentTokens.ToArray()));

							argumentTokens.Clear();
						}

						foreach (IToken token in parenthesizedExpression.Tokens)
						{
							if (token is CharLiteral { Value: ',' })
							{
								AddArgument();
							}
							else
							{
								argumentTokens.Add(token);
							}
						}

						AddArgument();

						tokens.Add(new InvocationExpression(identifier, arguments.ToArray()));
					}
					else
					{
						tokens.Add(parenthesizedExpression);
					}

					break;
				}

				case ',':
				{
					tokens.Add(new CharLiteral(','));

					break;
				}

				default:
				{
					if (char.IsWhiteSpace(c)) continue;

					if (Operator.TryGetOperator(c, i == 0 || tokens.Count == 0 || tokens[tokens.Count - 1] is not (DoubleLiteral or Identifier or IExpression), out Operator? op))
					{
						tokens.Add(op!);

						break;
					}

					if (char.IsDigit(c))
					{
						string doubleLiteralValueString = c.ToString();

						while (i + 1 < s.Length && char.IsDigit(c = s[i + 1]))
						{
							doubleLiteralValueString += c;

							i++;
						}

						if (i + 1 < s.Length && (c = s[i + 1]) == '.')
						{
							doubleLiteralValueString += c;

							i++;

							while (i + 1 < s.Length && char.IsDigit(c = s[i + 1]))
							{
								doubleLiteralValueString += c;

								i++;
							}
						}

						tokens.Add(new DoubleLiteral(double.Parse(doubleLiteralValueString)));

						break;
					}

					if (c == '_' || char.IsLetter(c))
					{
						string identifierValue = c.ToString();

						while (i + 1 < s.Length && char.IsLetterOrDigit(c = s[i + 1]))
						{
							identifierValue += c;

							i++;
						}

						if (identifierValue.All(static c => c == '_')) throw new Exception($"Invalid identifier '{identifierValue}'.");

						tokens.Add(new Identifier(identifierValue));

						break;
					}

					throw new Exception($"Unexpected character '{c}' at column {i}.");
				}
			}
		}

		return tokens.ToArray();
	}
}
