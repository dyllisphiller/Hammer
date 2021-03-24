using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
            // placeholders than there are objects to interpolate in the
            // System.String.Format() method, but not vice versa.

            // W1JDD runs Callook and his API terms are great. Thanks, Josh!
            { "us", "https://callook.info/{0}/json" },
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region">The region from which the license was issued.</param>
        /// <param name="callsign">The callsign to look up.</param>
        /// <param name="uri">When this method returns, contains the formatted URI; otherwise, returns null.</param>
        /// <returns>true if the Uri was successfully created; otherwise, false.</returns>
        public static bool TryMakeUri(string region, string callsign, out Uri uri)
        {
            string _uri;
            try
            {
                if (!ApiUriFormulary.TryGetValue(region, out string formula))
                {
                    throw new ArgumentOutOfRangeException(nameof(region), $"The API formula for {region} could not be found.");
                }

                _uri = string.Format(System.Globalization.CultureInfo.InvariantCulture, formula, callsign);
                uri = new Uri(_uri, UriKind.Absolute);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                uri = null;
                return false;
            }
        }
        
        public async static Task<JObject> GetLicenseJObjectAsync(Uri uri)
        {
            JObject jResult = null;

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                    HttpResponseMessage httpResponse = await client.GetAsync(uri);
                    httpResponse.EnsureSuccessStatusCode();
                    string result = await httpResponse.Content.ReadAsStringAsync();
                    jResult = JObject.Parse(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            return jResult;
        }
    }
}
