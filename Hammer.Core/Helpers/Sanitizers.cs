using System;
using System.Text.RegularExpressions;

namespace Hammer.Core.Helpers
{
    public static partial class Sanitizers
    {
        public static string SanitizeCallsign(string callsign)
        {
            if (string.IsNullOrWhiteSpace(callsign))
            {
                throw new ArgumentNullException(nameof(callsign), "callsign cannot be null, empty, or white space");
            }
            return Regex.Replace(callsign.ToUpperInvariant(), @"[^\p{Lu}\p{Nd}]", "");
        }
    }
}
