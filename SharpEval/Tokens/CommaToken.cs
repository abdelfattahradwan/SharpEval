namespace SharpEval.Tokens
{
	public sealed class CommaToken : IToken
	{
		public static readonly CommaToken Instance = new CommaToken();

		private CommaToken()
		{

		}
	}
}
