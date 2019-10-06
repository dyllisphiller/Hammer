using System;
using System.Collections.Generic;

namespace Hammer.Core.WebServices
{
    public static class APIs
    {
        /// <summary>
        /// Encapsulates strings with placeholders from which Uris are derived
        /// using <c>System.String.Format()</c>.
        /// </summary>
        public static readonly IDictionary<string, string> ApiUriFormulary = new Dictionary<string, string>
        {
            // Entries here MUST be constants and use the following placeholders:
            // {0} = callsign
            //
            // You do not have to use all of the placeholders. You can use fewer
            // placeholders than objects to interpolate in the method
            // System.String.Format() method, but not vice versa.
            { "us", "https://callook.info/{0}/json" },
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiFormula">The ApiUriFormulary string to format.</param>
        /// <param name="callsign">The callsign to search for.</param>
        /// <param name="uri">When this method returns, contains the formatted URI; otherwise, returns null.</param>
        /// <returns>true if the Uri was successfully created; otherwise, false.</returns>
        public static bool TryMakeUri(string apiFormula, string callsign, out Uri uri)
        {
            string _uri = null;
            try
            {
                _uri = string.Format(System.Globalization.CultureInfo.InvariantCulture, apiFormula, callsign);
                uri = new Uri(_uri);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debugger.Log(0, "WARN", ex.Message);
                uri = null;
                return false;
            }
        }
    }
}
