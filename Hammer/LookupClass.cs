using System;
using System.Text.RegularExpressions;

namespace Hammer.Callsigns.Search
{
    /// <summary>
    /// Defines a method for fetching callsign data.
    /// </summary>
    interface ILookupProvider
    {
        //void TryParse();
    }

    interface IExternalLookupProvider : ILookupProvider
    {

    }

    interface IInternalLookupProvider : ILookupProvider
    {

    }

    /// <summary>
    /// Utilities for getting data from Hammer or external stores.
    /// </summary>
    public class Lookup
    {
        /// <summary>
        /// Determines a callsign's country of issue.
        /// </summary>
        /// <param name="callsign">The amateur radio callsign to check.</param>
        /// <remarks>This currently only yields the United States or other.</remarks>
        private void GetCountryByITUPrefix(string callsign)
        {
            throw new NotImplementedException(nameof(GetCountryByITUPrefix));

            // string tokens = Hammer.Callsigns.Parser.Tokenizer(callsign).Item2;
        }

        public void GetCallsignByCountry(string callsign, int? country)
        {
            throw new NotImplementedException(nameof(GetCallsignByCountry));
        }

        /// <summary>
        /// Gets the callsign's issuing country.
        /// </summary>
        /// <param name="prefix">The callsign's prefix.</param>
        /// <returns>Two-letter country code of the issuing country.</returns>
        public static string CallsignCountryOfIssue(string prefix)
        {
            throw new NotImplementedException();
        }
        /*
            prefix = "1B3";

            Queue<Char> prefixQ = new Queue<Char>();

            foreach (char prefixChar in prefix.ToCharArray())
            {
                prefixQ.Enqueue(prefixChar);
            }

            for (int i = 0; i < prefixQ.Count(); i++)
            {
                char theChar = prefixQ.Dequeue();
                
            }
        }
        */
    }
}