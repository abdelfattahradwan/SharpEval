using System;
using System.Collections.Generic;
using System.Linq;

using SharpEval.Tokens;

namespace SharpEval
{
	public static class Interpreter
	{
		private static bool IsOperator(char @char)
		{
			switch (@char)
			{
				case '+':
				case '-':
				case '*':
				case '/':
				case '%':
				case '^':
				case '(':
				case '[':

					return true;

				default:

					return false;
			}
		}

		public static readonly Dictionary<string, Func<double>> Variables = new Dictionary<string, Func<double>>
		{
			{ "pi", () => Math.PI },
			{ "tau", () => Math.PI * 2.0d },
			{ "e", () => Math.E },
		};

		public static readonly Dictionary<string, Func<double[], double>> Functions = new Dictionary<string, Func<double[], double>>
		{
			{ "sin", args => Math.Sin(args[0]) },
			{ "cos", args => Math.Cos(args[0]) },
			{ "tan", args => Math.Tan(args[0]) },
		};

		public static Queue<IToken> Tokenize(string expression)
		{
			var output = new Queue<IToken>();

			var symbols = expression.Where(@char => !char.IsWhiteSpace(@char)).ToArray();

			for (var i = 0; i < symbols.Length; i++)
			{
				var symbol = symbols[i];

				if (char.IsDigit(symbol))
				{
					output.Enqueue(new DigitToken(symbol));
				}
				else if (char.IsLetter(symbol))
				{
					output.Enqueue(new LetterToken(symbol));
				}
				else
				{
					switch (symbol)
					{
						case '.':

							output.Enqueue(PeriodToken.Instance);

							break;

						case ',':

							output.Enqueue(CommaToken.Instance);

							break;

						case '+':

							if (i == 0 || IsOperator(symbols[i - 1])) continue;

							output.Enqueue(PlusToken.Instance);

							break;

						case '-':

							if (i == 0 || IsOperator(symbols[i - 1]))
							{
								output.Enqueue(UnaryMinusToken.Instance);
							}
							else output.Enqueue(MinusToken.Instance);

							break;

						case '*':

							output.Enqueue(TimesToken.Instance);

							break;

						case '/':

							output.Enqueue(DivideToken.Instance);

							break;

						case '%':

							output.Enqueue(ModuloToken.Instance);

							break;

						case '^':

							output.Enqueue(ExponentToken.Instance);

							break;

						case '(':

							if (i > 0 && char.IsLetter(symbols[i - 1]))
							{
								output.Enqueue(FunctionLeftParenthesisToken.Instance);
							}
							else output.Enqueue(LeftParenthesisToken.Instance);

							break;

						case ')':

							output.Enqueue(RightParenthesisToken.Instance);

							break;
					}
				}
			}

			return output;
		}

		public static Queue<IToken> NumberPass(Queue<IToken> tokens)
		{
			var output = new Queue<IToken>();

			while (tokens.Any())
			{
				var current = tokens.Dequeue();

				if (!(current is DigitToken || current is PeriodToken))
				{
					output.Enqueue(current);

					continue;
				}

				var numberSymbols = new Queue<IToken>();

				numberSymbols.Enqueue(current);

				while (tokens.Any())
				{
					var next = tokens.Peek();

					if (next is DigitToken)
					{
						numberSymbols.Enqueue(next);
					}
					else if (next is PeriodToken)
					{
						if (!numberSymbols.OfType<PeriodToken>().Any()) numberSymbols.Enqueue(next);
					}
					else if (next is LetterToken letterToken)
					{
						if (!numberSymbols.OfType<LetterToken>().Any())
						{
							if (letterToken.Letter == 'e')
							{
								numberSymbols.Enqueue(letterToken);
							}
							else break;
						}
					}
					else break;

					_ = tokens.Dequeue();
				}

				if (numberSymbols.Count > 1 || !(numberSymbols.Peek() is PeriodToken)) output.Enqueue(new NumberToken(numberSymbols));
			}

			return output;
		}

		public static Queue<IToken> WordPass(Queue<IToken> tokens)
		{
			var output = new Queue<IToken>();

			while (tokens.Any())
			{
				var current = tokens.Dequeue();

				if (!(current is LetterToken))
				{
					output.Enqueue(current);

					continue;
				}

				var wordSymbols = new Queue<IToken>();

				wordSymbols.Enqueue(current);

				while (tokens.Any())
				{
					var next = tokens.Peek();

					if (next is LetterToken || next is DigitToken)
					{
						wordSymbols.Enqueue(next);
					}
					else if (next is NumberToken numberToken)
					{
						foreach (var symbol in numberToken.Symbols) wordSymbols.Enqueue(symbol);
					}
					else break;

					_ = tokens.Dequeue();
				}

				output.Enqueue(new WordToken(wordSymbols));
			}

			return output;
		}

		private static ParameterArrayToken ExtractParameterArray(Queue<IToken> tokens)
		{
			var arrayElements = new Queue<ExpressionToken>();

			var elementTokens = new Queue<IToken>();

			while (tokens.Any())
			{
				var token = tokens.Dequeue();

				if (token is FunctionLeftParenthesisToken)
				{
					elementTokens.Enqueue(ExtractParameterArray(tokens));
				}
				else if (token is LeftParenthesisToken)
				{
					elementTokens.Enqueue(ExtractExpression(tokens));
				}
				else if (token is CommaToken)
				{
					if (!elementTokens.Any()) continue;

					arrayElements.Enqueue(new ExpressionToken(InfixToPostfix(elementTokens)));

					elementTokens.Clear();
				}
				else if (token is RightParenthesisToken)
				{
					if (elementTokens.Any()) arrayElements.Enqueue(new ExpressionToken(InfixToPostfix(elementTokens)));

					break;
				}
				else elementTokens.Enqueue(token);
			}

			return new ParameterArrayToken(arrayElements);
		}

		public static Queue<IToken> ParameterArrayPass(Queue<IToken> tokens)
		{
			var output = new Queue<IToken>();

			while (tokens.Any())
			{
				var token = tokens.Dequeue();

				if (token is FunctionLeftParenthesisToken)
				{
					output.Enqueue(ExtractParameterArray(tokens));
				}
				else output.Enqueue(token);
			}

			return output;
		}

		private static ExpressionToken ExtractExpression(Queue<IToken> tokens)
		{
			var expressionTokens = new Queue<IToken>();

			while (tokens.Any())
			{
				var token = tokens.Dequeue();

				if (token is LeftParenthesisToken)
				{
					expressionTokens.Enqueue(ExtractExpression(tokens));
				}
				else if (token is FunctionLeftParenthesisToken)
				{
					expressionTokens.Enqueue(ExtractParameterArray(tokens));
				}
				else if (token is RightParenthesisToken)
				{
					break;
				}
				else expressionTokens.Enqueue(token);
			}

			return new ExpressionToken(InfixToPostfix(expressionTokens));
		}

		public static Queue<IToken> ExpressionPass(Queue<IToken> tokens)
		{
			var output = new Queue<IToken>();

			while (tokens.Any())
			{
				var token = tokens.Dequeue();

				if (token is LeftParenthesisToken)
				{
					output.Enqueue(ExtractExpression(tokens));
				}
				else output.Enqueue(token);
			}

			return output;
		}

		private static int PrecedenceOf(IToken token)
		{
			if (token is PlusToken || token is MinusToken)
			{
				return 0;
			}
			else if (token is TimesToken || token is DivideToken || token is ModuloToken)
			{
				return 1;
			}
			else if (token is ExponentToken)
			{
				return 2;
			}
			else if (token is UnaryMinusToken)
			{
				return 3;
			}
			else if (token is ExpressionToken)
			{
				return 4;
			}
			else throw new ArgumentException($"Invalid token type '{token.GetType().Name}'.", nameof(token));
		}

		public static Queue<IToken> InfixToPostfix(Queue<IToken> tokens)
		{
			var output = new Queue<IToken>();

			var operatorStack = new Stack<IToken>();

			while (tokens.Any())
			{
				var token = tokens.Dequeue();

				if (token is NumberToken || token is WordToken || token is ParameterArrayToken)
				{
					output.Enqueue(token);
				}
				else if (token is UnaryMinusToken && operatorStack.Any() && operatorStack.Peek() is UnaryMinusToken)
				{
					operatorStack.Push(token);
				}
				else
				{
					while (operatorStack.Any() && PrecedenceOf(operatorStack.Peek()) >= PrecedenceOf(token)) output.Enqueue(operatorStack.Pop());

					operatorStack.Push(token);
				}
			}

			while (operatorStack.Any()) output.Enqueue(operatorStack.Pop());

			return output;
		}

		public static Queue<IToken> Parse(string expression)
		{
			var output = Tokenize(expression);

			output = NumberPass(output);

			output = WordPass(output);

			output = ParameterArrayPass(output);

			output = ExpressionPass(output);

			return InfixToPostfix(output);
		}

		public static double Evaluate(Queue<IToken> tokens)
		{
			var operandStack = new Stack<double>();

			while (tokens.Any())
			{
				var token = tokens.Dequeue();

				double operand1;
				double operand2;

				if (token is NumberToken numberToken)
				{
					operandStack.Push(numberToken.Value);
				}
				else if (token is WordToken wordToken)
				{
					if (tokens.Any() && tokens.Peek() is ParameterArrayToken arrayToken)
					{
						operandStack.Push(Functions[wordToken.ToString()](arrayToken.Elements.Select(expression => Evaluate(expression.Tokens)).ToArray()));

						_ = tokens.Dequeue();
					}
					else operandStack.Push(Variables[wordToken.ToString()]());
				}
				else if (token is PlusToken)
				{
					operandStack.Push(operandStack.Pop() + operandStack.Pop());
				}
				else if (token is MinusToken)
				{
					operand2 = operandStack.Pop();
					operand1 = operandStack.Pop();

					operandStack.Push(operand1 - operand2);
				}
				else if (token is TimesToken)
				{
					operandStack.Push(operandStack.Pop() * operandStack.Pop());
				}
				else if (token is DivideToken)
				{
					operand2 = operandStack.Pop();
					operand1 = operandStack.Pop();

					operandStack.Push(operand1 / operand2);
				}
				else if (token is ModuloToken)
				{
					operand2 = operandStack.Pop();
					operand1 = operandStack.Pop();

					operandStack.Push(operand1 % operand2);
				}
				else if (token is ExponentToken)
				{
					operand2 = operandStack.Pop();
					operand1 = operandStack.Pop();

					operandStack.Push(Math.Pow(operand1, operand2));
				}
				else if (token is UnaryMinusToken)
				{
					operandStack.Push(-operandStack.Pop());
				}
				else if (token is ParameterArrayToken arrayToken)
				{
					operandStack.Push(arrayToken.Elements.Sum(expression => Evaluate(expression.Tokens)));
				}
				else if (token is ExpressionToken expressionToken)
				{
					operandStack.Push(Evaluate(expressionToken.Tokens));
				}
			}

			return operandStack.Pop();
		}

		public static double ParseAndEvaluate(string expression) => Evaluate(Parse(expression));
	}
}
