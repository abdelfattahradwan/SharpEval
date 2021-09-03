namespace SharpEval.Tokens
{
	public sealed class LetterToken : IToken
	{
		public char Letter { get; }

		public LetterToken(char letter) => Letter = letter;

		public override string ToString() => Letter.ToString();
	}
}
