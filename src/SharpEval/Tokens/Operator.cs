namespace SharpEval.Tokens;

public sealed class Operator : IToken
{
	public char Symbol { get; }

	public bool IsUnary { get; }

	public Operator(char symbol, bool isUnary)
	{
		Symbol = symbol;

		IsUnary = isUnary;
	}

	public static readonly Operator Plus = new('+', false);
	public static readonly Operator Minus = new('-', false);

	public static readonly Operator Times = new('*', false);
	public static readonly Operator Divide = new('/', false);

	public static readonly Operator UnaryPlus = new('+', true);
	public static readonly Operator UnaryMinus = new('-', true);

	public static bool TryGetOperator(char symbol, bool isUnary, out Operator? op)
	{
		op = symbol switch
		{
			'+' => isUnary ? UnaryPlus : Plus,
			'-' => isUnary ? UnaryMinus : Minus,

			'*' => Times,
			'/' => Divide,

			_ => null,
		};

		return op != null;
	}
}
