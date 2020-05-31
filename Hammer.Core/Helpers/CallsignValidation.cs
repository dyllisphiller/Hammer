using System;
using Hammer.Core.Callsigns;

namespace Hammer.Core.Callsigns
{
    /// <summary>
    /// Provides static methods for checking callsigns against the ITU callsign specification.
    /// </summary>
    /// <remarks>
    /// A callsign can be both valid and not issued.
    /// Unless otherwise stated, methods here <i>do not</i> check callsigns against licensing databases.
    /// </remarks>
    public static class Validation
    {
        /// <summary>
        /// Determines whether a callsign has a known prefix.
        /// </summary>
        /// <param name="callsign">The callsign to validate.</param>
        /// <returns>true if the callsign has a known prefix; otherwise, false</returns>
        public static bool IsKnownPrefix(string callsign)
        {
            string _region;
            return Prefixes.TryGetRegion(callsign, out _region);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether a callsign matches the ITU's standard forhas a known prefix <i>and</i> conforms to the issuer's known callsign patterns.
        /// </summary>
        /// <param name="callsign">The callsign to validate.</param>
        /// <returns>true if the callsign meets the ITU specification; otherwise, false</returns>
        public static bool IsValid(string callsign)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validates a callsign against an issuer's known callsign patterns.
        /// </summary>
        /// <param name="callsign">The callsign to validate.</param>
        //public static bool IsValidForRegion(string callsign, string region)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
