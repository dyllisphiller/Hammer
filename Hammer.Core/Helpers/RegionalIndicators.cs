using System;
using System.Collections.Generic;

namespace Hammer.Core.Helpers
{
    public static class RegionalIndicators
    {
        /// <summary>
        /// A dictionary with char A-Z as keys and their equivalent Unicode
        /// Regional Indicator Symbols as values; the latter are represented as 
        /// integers of the UTF-32 code points.
        /// </summary>
        public static IDictionary<char, int> SymbolPairs { get; } = new Dictionary<char, int>
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
        /// <exception cref="System.ArgumentOutOfRangeException">region is not two letters.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
        public static bool TryGetIndicatorPair(string region, out string indicator)
        {
            char[] letters = (region.Length == 2) ?
                region.ToUpperInvariant().ToCharArray()
                : throw new ArgumentOutOfRangeException(nameof(region), "TryGetIndicatorPair(): region must be two letters.");

            // the method-internal representation of `indicator`;
            // all code paths must set indicator = _indicator
            string _indicator = null;

            foreach (char letter in letters)
            {
                int codepoint;
                string symbol;

                // get the Regional Indicator equivalent of the letter from the dictionary
                SymbolPairs.TryGetValue(letter, out codepoint);

                // make this a character instead of its UTF-32 integer equivalent
                symbol = Char.ConvertFromUtf32(codepoint);

                // append the character to _indicator
                _indicator += symbol;
            }

            // pass the method-internal _indicator string to the output indicator string
            indicator = _indicator;

            // if things haven't exploded by now, indicate success
            return true;
        }
    }
}
