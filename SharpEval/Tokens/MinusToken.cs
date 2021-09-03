namespace SharpEval.Tokens
{
	public sealed class MinusToken : IToken
	{
		public static readonly MinusToken Instance = new MinusToken();

		private MinusToken()
		{

		}
	}
}
