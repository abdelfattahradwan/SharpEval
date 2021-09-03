namespace SharpEval.Tokens
{
	public sealed class FunctionLeftParenthesis : IToken
	{
		public static readonly FunctionLeftParenthesis Instance = new FunctionLeftParenthesis();

		private FunctionLeftParenthesis()
		{

		}
	}
}
