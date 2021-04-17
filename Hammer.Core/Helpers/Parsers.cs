using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hammer.Core.Callsigns;
using Hammer.Core.Models;
using Hammer.Core.Models.Converters;
using Hammer.Core.Models.Regional;
using Hammer.Core.WebServices;

namespace Hammer.Core.Helpers
{
    public static class Parsers
    {
        public static async Task<BaseLicense> GetLicenseFromJsonAsync(string callsign)
        {
            // Check and sanitize the callsign input.
            callsign = Sanitizers.SanitizeCallsign(callsign);

            // Get the callsign's region.
            Issuers.TryGetRegion(callsign, out string region);

            // Right now, only US callsigns are supported.
            if (region != "us")
            {
                throw new UnsupportedIssuerException("Hammer can only look up US callsigns");
            }

            APIs.TryMakeUri(region, callsign, out Uri licenseDataUri);

            using (HttpClient client = new HttpClient())
            {
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
                    throw;
                }
            }
        }

    }
}
