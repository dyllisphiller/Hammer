using System;
using System.Collections.Generic;
using System.Linq;

namespace Hammer.Core.Helpers
{
    public static class RegionalIndicators
    {
        /// <summary>
        /// A dictionary with char A-Z as keys and their equivalent Unicode
        /// Regional Indicator Symbols’ UTF-32 representations as values.
        /// </summary>
        internal static IDictionary<char, int> SymbolPairs { get; } = new Dictionary<char, int>
        {
            { 'A', 0x1F1E6 },
            { 'B', 0x1F1E7 },
            { 'C', 0x1F1E8 },
            { 'D', 0x1F1E9 },
            { 'E', 0x1F1EA },
            { 'F', 0x1F1EB },
            { 'G', 0x1F1EC },
            { 'H', 0x1F1ED },
            { 'I', 0x1F1EE },
            { 'J', 0x1F1EF },
            { 'K', 0x1F1F0 },
            { 'L', 0x1F1F1 },
            { 'M', 0x1F1F2 },
            { 'N', 0x1F1F3 },
            { 'O', 0x1F1F4 },
            { 'P', 0x1F1F5 },
            { 'Q', 0x1F1F6 },
            { 'R', 0x1F1F7 },
            { 'S', 0x1F1F8 },
            { 'T', 0x1F1F9 },
            { 'U', 0x1F1FA },
            { 'V', 0x1F1FB },
            { 'W', 0x1F1FC },
            { 'X', 0x1F1FD },
            { 'Y', 0x1F1FE },
            { 'Z', 0x1F1FF },
        };

        /// <summary>
        /// Takes a two-letter region code and outputs the equivalent Unicode Regional Indicator Symbol.
        /// </summary>
        /// <param name="region">An ISO 3166-1 alpha-2 country code.</param>
        /// <param name="indicator">When this method returns, the <c>region</c>-equivalent Regional Indicator Symbol.</param>
        /// <remarks>
        /// This method does not check if the region passed is a real region.
        /// There is no region AA, but the method will return its Regional Indicator Symbol equivalent anyway.
        /// </remarks>
        /// <returns>true if successful; otherwise, false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">region is not two letters.</exception>
        public static bool TryGetIndicatorPair(string region, out string indicator)
        {
            if (string.IsNullOrEmpty(region) || region.Length != 2)
            {
                indicator = null;
                return false;
            }

            char[] letters = region.ToUpperInvariant().ToCharArray();

            // the method-internal representation of `indicator`
            string _indicator = null;

            foreach (char c in letters)
            {
                // get the Regional Indicator pair from the dictionary
                KeyValuePair<char, int> codepoint = SymbolPairs.FirstOrDefault(s => s.Key == c);

                // convert the UTF-32 integer to a character
                string symbol = char.ConvertFromUtf32(codepoint.Value);

                // append the character to _indicator
                _indicator += symbol;
            }

            indicator = _indicator;
            return true;
        }
    }
}
