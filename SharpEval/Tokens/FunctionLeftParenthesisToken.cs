namespace SharpEval.Tokens
{
	public sealed class FunctionLeftParenthesisToken : IToken
	{
		public static readonly FunctionLeftParenthesisToken Instance = new FunctionLeftParenthesisToken();

		private FunctionLeftParenthesisToken()
		{

		}
	}
}
