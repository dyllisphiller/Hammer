/*using System;
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
        public LicenseStatus Status { get; set; }
        public LicenseeTypes LicenseeType { get; set; }
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

        private static OperatorClasses ParseOperatorClass(string operatorClassString)
        {
            return !Enum.TryParse(operatorClassString, true, out OperatorClasses operatorClass)
                ? operatorClass
                : OperatorClasses.Unknown;
        }

        private static LicenseeTypes ParseLicenseeType(string licenseeTypeString)
        {
            return !Enum.TryParse(licenseeTypeString, true, out LicenseeTypes licenseeType)
                ? licenseeType
                : LicenseeTypes.Unknown;
        }

        private static LicenseStatus ParseLicenseStatus(string licenseStatusString)
        {
            return !Enum.TryParse(licenseStatusString, true, out LicenseStatus licenseStatus)
                ? licenseStatus
                : LicenseStatus.Unknown;
        }

        public USLicense(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;

                LicenseStatus _status = ParseLicenseStatus(root.GetProperty("status").GetString());

                if (_status == LicenseStatus.Valid)
                {
                    if (root.TryGetProperty("type", out JsonElement jsonLicenseeType))
                    {
                        string licenseeType = jsonLicenseeType.GetString().ToUpperInvariant();
                        LicenseeType = ParseLicenseeType(licenseeType);
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
                            Current.OperClass = ParseOperatorClass(currentOperClass);
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
                        else
                        {
                            Previous.Callsign = null;
                        }

                        if (jsonPrevious.TryGetProperty("operClass", out JsonElement jsonPreviousOperClass) && !string.IsNullOrWhiteSpace(jsonPreviousOperClass.GetString()))
                        {
                            string previousOperClass = jsonPreviousOperClass.GetString().ToUpperInvariant();
                            Previous.OperClass = ParseOperatorClass(previousOperClass);
                        }
                    }

                    if (root.TryGetProperty("trustee", out JsonElement jsonTrustee))
                    {
                        if (jsonTrustee.TryGetProperty("callsign", out JsonElement jsonTrusteeCallsign) && !string.IsNullOrWhiteSpace(jsonTrusteeCallsign.GetString()))
                        {
                            Trustee = new Callsign(jsonTrusteeCallsign.GetString());
                        }
                    }

                    if (root.TryGetProperty("name", out JsonElement jsonName)
                        && !string.IsNullOrEmpty(jsonName.GetString()))
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
                }
            }
        }
    }

    public class USLicenseData
    {
        public Callsign Callsign;
        public OperatorClasses OperClass;
    }
}
*/