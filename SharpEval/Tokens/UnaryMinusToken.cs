namespace SharpEval.Tokens
{
	public sealed class UnaryMinusToken : IToken
	{
		public static readonly UnaryMinusToken Instance = new UnaryMinusToken();

		private UnaryMinusToken()
		{

		}
	}
}
