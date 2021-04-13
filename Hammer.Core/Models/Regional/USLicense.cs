using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hammer.Core.Maps;

namespace Hammer.Core.Models.Regional
{
    public class USLicense
    {
        public USStatus Status { get; set; }
        public string LicenseType { get; set; }
        public USLicenseData Current { get; set; }
        public USLicenseData Previous { get; set; }
        public Callsign Trustee { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressAttn { get; set; }
        public GeographicPoint Location { get; set; }
        public string GridSquare { get; set; }
        public DateTimeOffset GrantDate { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public DateTimeOffset LastActionDate { get; set; }
        public string Frn { get; set; }
        public Uri UlsUri { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }

        public USLicense(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;

                string[] validLicenseTypes = new string[] { "PERSON", "CLUB", "RECREATION", "RACES", "MILITARY" };
                string[] validOperatorClasses = new string[] { "TECHNICIAN", "GENERAL", "EXTRA", "NOVICE", "TECHNICIAN PLUS", "ADVANCED" };

                string status = root.GetProperty("status").GetString().ToUpperInvariant();

                switch (status)
                {
                    case "INVALID":
                        Status = USStatus.Invalid;
                        break;

                    case "UPDATING":
                        Status = USStatus.Updating;
                        break;

                    case "VALID":
                        Status = USStatus.Valid;
                        JsonElement current = root.GetProperty("current");
                        
                        Callsign currentCallsign = new Callsign(current.GetProperty("callsign").GetString().ToUpperInvariant());

                        if (root.TryGetProperty("type", out JsonElement jsonLicenseType))
                        {
                            string licenseType = jsonLicenseType.GetString().ToUpperInvariant();
                            LicenseType = validLicenseTypes.Any(s => licenseType.Equals(s)) ? licenseType : "";
                        }

                        if (root.TryGetProperty("current", out JsonElement jsonCurrent))
                        {
                            Current = new USLicenseData();

                            if (jsonCurrent.TryGetProperty("callsign", out JsonElement jsonCurrentCallsign))
                            {
                                Current.Callsign = new Callsign(jsonCurrentCallsign.GetString().ToUpperInvariant());
                            }

                            if (jsonCurrent.TryGetProperty("operClass", out JsonElement jsonCurrentOperClass))
                            {
                                string currentOperClass = jsonCurrentOperClass.GetString().ToUpperInvariant();
                                Current.OperClass = validOperatorClasses.Any(s => currentOperClass.Equals(s)) ? currentOperClass : "";
                            }
                        }

                        // If there is a previous callsign and/or operator class, add Previous data
                        if (root.TryGetProperty("previous", out JsonElement jsonPrevious))
                        {
                            Previous = new USLicenseData();

                            if (jsonPrevious.TryGetProperty("callsign", out JsonElement jsonPreviousCallsign) && !string.IsNullOrWhiteSpace(jsonPreviousCallsign.GetString()))
                            {
                                Previous.Callsign = new Callsign(jsonPreviousCallsign.GetString().ToUpperInvariant());
                            }

                            if (jsonPrevious.TryGetProperty("operClass", out JsonElement jsonPreviousOperClass) && !string.IsNullOrWhiteSpace(jsonPreviousOperClass.GetString()))
                            {
                                string previousOperClass = jsonPreviousOperClass.GetString().ToUpperInvariant();
                                Previous.OperClass = validOperatorClasses.Any(s => previousOperClass.Equals(s)) ? previousOperClass : "";
                            }
                        }

                        if (root.TryGetProperty("trustee", out JsonElement jsonTrustee))
                        {
                            if (jsonTrustee.TryGetProperty("callsign", out JsonElement jsonTrusteeCallsign))
                            {
                                Trustee = new Callsign(jsonTrusteeCallsign.GetString());
                            }
                        }

                        if (root.TryGetProperty("name", out JsonElement jsonName))
                        {
                            Name = jsonName.GetString();
                        }

                        if (root.TryGetProperty("address", out JsonElement jsonAddress))
                        {
                            AddressAttn = jsonAddress.TryGetProperty("attn", out JsonElement jsonAddressAttn) ? jsonAddressAttn.GetString() : "";
                            AddressLine1 = jsonAddress.TryGetProperty("line1", out JsonElement jsonAddressLine1) ? jsonAddressLine1.GetString() : "";
                            AddressLine2 = jsonAddress.TryGetProperty("line2", out JsonElement jsonAddressLine2) ? jsonAddressLine2.GetString() : "";
                        }

                        if (root.TryGetProperty("location", out JsonElement jsonLocation))
                        {
                            Location = new GeographicPoint
                            {
                                Latitude = jsonLocation.TryGetProperty("latitude", out JsonElement jsonLocationLatitude)
                                    ? Convert.ToDouble(jsonLocationLatitude.GetString(), CultureInfo.GetCultureInfo("en-US"))
                                    : 0,
                                Longitude = jsonLocation.TryGetProperty("longitude", out JsonElement jsonLocationLongitude)
                                    ? Convert.ToDouble(jsonLocationLongitude.GetString(), CultureInfo.GetCultureInfo("en-US"))
                                    : 0,
                            };
                            GridSquare = jsonLocation.TryGetProperty("gridsquare", out JsonElement jsonLocationGridSquare) ? jsonLocationGridSquare.GetString() : "";
                        }

                        if (root.TryGetProperty("otherInfo", out JsonElement jsonOtherInfo))
                        {
                            if (jsonOtherInfo.TryGetProperty("grantDate", out JsonElement jsonGrantDate))
                            {
                                GrantDate = DateTimeOffset.TryParse(jsonGrantDate.GetString(), out DateTimeOffset _date)
                                    ? _date
                                    : new DateTimeOffset();
                            }

                            if (jsonOtherInfo.TryGetProperty("expiryDate", out JsonElement jsonExpiryDate))
                            {
                                ExpiryDate = DateTimeOffset.TryParse(jsonExpiryDate.GetString(), out DateTimeOffset _date)
                                    ? _date
                                    : new DateTimeOffset();
                            }

                            if (jsonOtherInfo.TryGetProperty("lastActionDate", out JsonElement jsonLastActionDate))
                            {
                                LastActionDate = DateTimeOffset.TryParse(jsonLastActionDate.GetString(), out DateTimeOffset _date)
                                    ? _date
                                    : new DateTimeOffset();
                            }

                            if (jsonOtherInfo.TryGetProperty("frn", out JsonElement jsonFrn))
                            {
                                Frn = jsonFrn.GetString();
                            }

                            if (jsonOtherInfo.TryGetProperty("ulsUrl", out JsonElement jsonUlsUrl))
                            {
                                UlsUri = new Uri(jsonUlsUrl.GetString());
                            }
                        }

                        break;

                    default:
                        throw new Exception("Returned JSON data does not match API specification.");
                }
            }
        }
    }

    public enum USStatus
    {
        Valid,
        Invalid,
        Updating,
    }

    public enum USOperatorClass
    {
        Technician,
        General,
        Extra,
        // these are no longer issued but may still be used on certain licenses
        Novice,
        TechnicianPlus,
        Advanced,
    }

    public class USLicenseData
    {
        public Callsign Callsign;
        public string OperClass;
    }
}
