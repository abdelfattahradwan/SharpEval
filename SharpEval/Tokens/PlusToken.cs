namespace SharpEval.Tokens
{
	public sealed class PlusToken : IToken
	{
		public static readonly PlusToken Instance = new PlusToken();

		private PlusToken()
		{

		}
	}
}
