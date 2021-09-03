namespace SharpEval.Tokens
{
	public sealed class RightParenthesisToken : IToken
	{
		public static readonly RightParenthesisToken Instance = new RightParenthesisToken();

		private RightParenthesisToken()
		{

		}
	}
}
