using System;

namespace Hammer.Core.Callsigns
{
    public static class Validation
    {
        /// <summary>
        /// Determines whether a callsign has a valid ITU prefix <i>and</i> conforms to the issuer's known callsign patterns.
        /// </summary>
        /// <param name="callsign">The callsign to validate.</param>
        public static bool IsValid(string callsign)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validates a callsign against an issuer's known callsign patterns.
        /// </summary>
        /// <param name="callsign">The callsign to validate.</param>
        /// <param name="region">The callsign's country/region of issue.</param>
        public static bool IsValidForRegion(string callsign, string region)
        {
            throw new NotImplementedException();
        }
    }
}
