using Hammer.Core.Callsigns;
using Hammer.Core.Models;
using Hammer.Core.Models.Converters.US;
using Hammer.Core.WebServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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

            HttpClient client = new HttpClient();
            APIs.TryMakeUri(region, callsign, out Uri licenseDataUri);


            try
            {
                client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
                };
                usLicenseTask = client.GetFromJsonAsync<USLicense>(licenseDataUri, options);
                USLicense usLicense = await usLicenseTask;
                resultTask = usLicense.ToLicenseAsync();
                await resultTask;
                if (resultTask.Result == null) throw new ApplicationException("Could not get or parse License from API.");
                return resultTask.Result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
            
        }

    }
}
