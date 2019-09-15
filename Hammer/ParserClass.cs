using Hammer.Callsigns.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hammer.Callsigns
{

    /// <summary>
    /// Provides methods to extract parts of a callsign.
    /// </summary>
    class Parser //: IParser
    {
        /// <summary>
        /// Tokenizes (lexes) callsigns into a triple of the callsign, prefix, and suffix.
        /// </summary>
        /// <param name="callsign">The callsign to tokenize.</param>
        /// <returns>A triple consisting of the callsign, prefix, and suffix</returns>
        protected static Tuple<string, string, string> Tokenizer(string callsign)
        {
            if (callsign[0] == 'Q')
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(callsign), message: "No amateur callsigns begin with the letter Q.");
            }
            else
            {
                // doin' a heckin' regex
                Regex _rgx = new Regex(@"^(?<prefix>[0-PR-pr-z][0-z]{1,3})(?<suffix>\d+[0-z]+)$", RegexOptions.Compiled);
                Match match = _rgx.Match(callsign);

                var matches = new Tuple<string, string, string>(match.Value, match.Groups["prefix"].Value, match.Groups["suffix"].Value);

                // This is for debugging.
                // Console.WriteLine("Callsign {0} has a prefix of {1} and a suffix of {2}.", matches.Item1, matches.Item2, matches.Item3);

                return matches;
            }
        }

        public static bool IsCallsignIssuedByUnitedStates(string callsign)
        {
            Regex callsignPattern = new Regex(@"^(A[A-L].*|[KNW].*)$", RegexOptions.IgnoreCase);
            return callsignPattern.IsMatch(callsign);
        }
    }
}