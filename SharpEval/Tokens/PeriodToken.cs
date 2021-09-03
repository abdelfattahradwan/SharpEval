namespace SharpEval.Tokens
{
	public sealed class PeriodToken : IToken
	{
		public static readonly PeriodToken Instance = new PeriodToken();

		private PeriodToken()
		{

		}

		public override string ToString() => ".";
	}
}
