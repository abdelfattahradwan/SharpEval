namespace SharpEval.Tokens
{
	public sealed class LeftParenthesisToken : IToken
	{
		public static readonly LeftParenthesisToken Instance = new LeftParenthesisToken();

		private LeftParenthesisToken()
		{

		}
	}
}
