using System;
using System.Text.RegularExpressions;
using Hammer.Core.Callsigns;
using Hammer.Core.Models;

namespace Hammer.Core.Callsigns
{
    /// <summary>
    /// Provides static methods for manipulating callsigns.
    /// </summary>
    /// <remarks>
    /// A callsign can be both valid and not issued.
    /// These methods <i>do not</i> check callsigns against licensing databases.
    /// </remarks>
    public static partial class Helpers
    {
        /// <summary>
        /// Determines whether a callsign has a known prefix.
        /// </summary>
        /// <param name="callsign">The callsign to validate.</param>
        /// <returns>true if the callsign has a known prefix; otherwise, false</returns>
        public static bool HasKnownPrefix(string callsign)
        {
            return Prefixes.TryGetRegion(callsign, out _);
        }

        /// <summary>
        /// Validates a callsign against an issuer's prefix.
        /// </summary>
        /// <param name="callsign">The callsign to validate.</param>
        public static bool IsValidForRegion(string callsign, string region)
        {
            return Prefixes.TryGetRegion(callsign, out string _region) && _region == region;
        }

        public static string SanitizeCallsign(this string callsign)
        {
            if (string.IsNullOrWhiteSpace(callsign))
            {
                throw new ArgumentNullException(nameof(callsign), "callsign cannot be null, empty, or white space");
            }
            return Regex.Replace(callsign.ToUpperInvariant(), @"[^\p{Lu}\p{Nd}]", "");
        }

        public static Callsign ToCallsign(this string sign)
        {
            return new Callsign(sign.SanitizeCallsign());
        }
    }
}
