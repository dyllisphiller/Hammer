using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hammer.Core.Models;
using Hammer.Core.WebServices;

namespace Hammer.Core.Helpers
{
    public static partial class Parsers
    {
        // TODO: Move this, probably into the SearchPage code-behind.
        public static async Task<BaseLicense> GetLicenseFromJsonAsync(Callsign callsign)
        {
            // Right now, only US callsigns are supported.
            if (callsign.Region != "us")
            {
                throw new UnsupportedIssuerException("Hammer can only look up US callsigns");
            }

            APIs.TryMakeUri(callsign, out Uri licenseDataUri);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                    HttpResponseMessage httpResponse = await client.GetAsync(licenseDataUri);
                    httpResponse.EnsureSuccessStatusCode();
                    BaseLicense licenseResult = Factories.MakeLicense(await httpResponse.Content.ReadAsStringAsync());
                    return licenseResult;
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
