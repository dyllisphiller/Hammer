using System;
using System.Collections.Generic;

namespace Hammer.Core.Helpers
{
    public static class RegionalIndicators
    {
        /// <summary>
        /// Encapsulation of KeyValuePairs where the 26 keys are capital Latin
        /// letters A-Z and their respective values are each letter's
        /// equivalent Unicode Regional Indicator Symbol represented as UTF-32
        /// integer code points.
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
        /// Gets an indicator pair
        /// </summary>
        /// <param name="region">An ISO 3166-1 alpha-2 country code.</param>
        /// <param name="indicator">The <c>region</c>-equivalent Regional Indicator Symbols.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
        public static void TryGetIndicatorPair(string region, out string indicator)
        {
            char[] letters = (region.Length == 2) ?
                region.ToUpperInvariant().ToCharArray()
                : throw new ArgumentOutOfRangeException();

            // the method-internal representation of the `out string indicator`
            // parameter; all code paths must set indicator = _indicator
            string _indicator = null;

            foreach (char letter in letters)
            {
                int codepoint;
                string symbol;

                SymbolPairs.TryGetValue(letter, out codepoint);
                symbol = Char.ConvertFromUtf32(codepoint);

                _indicator += symbol;
            }

            indicator = _indicator;
        }
    }
}
