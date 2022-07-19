using System.Linq;

namespace SharpEval.Utilities;

public static class StringUtilities
{
	public static bool IsNullOrWhiteSpace(string? s)
	{
		return s == null || s.All(char.IsWhiteSpace);
	}

	public static string Slice(string s, int start, int end)
	{
		return s.Substring(start, end - start);
	}
}
