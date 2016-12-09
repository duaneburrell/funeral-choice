using System;

namespace Obsequy.Utility
{
	public static class StringExtension
	{
		/// <summary>
		/// Scrubs the input string, removing leading and trailing whitespaces, and potentailly harmful characters for SQL injection
		/// </summary>
		/// <param name="input">the string to scrub</param>
		/// <returns>the scrubbed string</returns>
		public static string Scrub(this string input)
		{
			// TODO: add method to remove potentially harmful characters
			if (!string.IsNullOrEmpty(input))
			{
				return input.Trim();
			}
			return input;
		}
	}
}
