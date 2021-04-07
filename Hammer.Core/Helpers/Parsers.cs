using Hammer.Core.Callsigns;
using Hammer.Core.Models;
using Hammer.Core.Models.Regional;
using Hammer.Core.Models.Converters;
using Hammer.Core.WebServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hammer.Core.Helpers
{
    public static class Parsers
    {
        public static async Task<License> GetLicenseFromJsonAsync(string callsign)
        {
            callsign = Sanitizers.SanitizeCallsign(callsign);

            Prefixes.TryGetRegion(callsign, out string region);
            if (region != "us") throw new ApplicationException("Hammer can't look up non-FCC callsigns yet.");

            APIs.TryMakeUri(region, callsign, out Uri licenseDataUri);

            HttpClient client = new HttpClient();

            try
            {
                client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                HttpResponseMessage httpResponse = await client.GetAsync(licenseDataUri);
                httpResponse.EnsureSuccessStatusCode();
                USLicense usLicenseResult = new USLicense(await httpResponse.Content.ReadAsStringAsync());
                return usLicenseResult.ToLicense();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
