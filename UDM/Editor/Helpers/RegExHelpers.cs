using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UDM.Helpers {
	public class RegExHelpers {
		private const string ARGUMENT_PATTERN_FORMAT = "(?<={0}:)(\\S+)";
		private const string IP_FROM_ADB_PATTERN = "(?<=inet\\s).*?(?=\\/)";
		private const string FIRST_WORD_PATTERN = "(\\S+)";

		public static string GetFirstWord(string line) {
			return Match(FIRST_WORD_PATTERN, line);
		}
		
		public static string GetIPFromADBMessage(string msg) {
			return Match(IP_FROM_ADB_PATTERN, msg);
		}
		
		public static string GetArgument(string line, string argumentName) {
			var pattern = string.Format(ARGUMENT_PATTERN_FORMAT, argumentName);

			return Match(pattern, line);
		}
		
		private static string Match(string pattern, string query) {
			var rg    = new Regex(pattern);
			var match = rg.Match(query);
			return match.Success ? match.Value : null;
		}
	}
}