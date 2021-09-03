namespace SharpEval.Tokens
{
	public sealed class DigitToken : IToken
	{
		public char Digit { get; }

		public DigitToken(char digit) => Digit = digit;

		public override string ToString() => Digit.ToString();
	}
}
