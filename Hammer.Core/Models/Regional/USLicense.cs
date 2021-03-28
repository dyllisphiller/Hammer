using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hammer.Core.Models.Regional
{
    public class USLicense
    {
        public USStatus Status { get; set; }
        public USLicenseType LicenseType { get; set; }
        public USLicenseData Current { get; set; }
        public USLicenseData Previous { get; set; }
        public Callsign Trustee { get; set; }
        public string Name { get; set; }
        public USAddress Address { get; set; }
        public USLocation Location { get; set; }
        public USOtherInfo OtherInfo { get; set; }

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
                        JsonElement current = root.GetProperty("current");
                        Callsign currentCallsign = new Callsign(current.GetProperty("callsign").GetString().ToUpperInvariant());

                        // REWRITE CURRENT SECTION LIKE PREVIOUS SECTION BELOW
                        //Current = new USLicenseData()
                        //{
                        //    Callsign = new Callsign(current.GetProperty("callsign").GetString()),
                        //    OperClass = current.GetProperty("operClass").GetString(),
                        //};

                        // If there is a previous callsign and/or operator class, add Previous data
                        if (root.TryGetProperty("previous", out JsonElement jsonPrevious))
                        {
                            Previous = new USLicenseData();

                            if (jsonPrevious.TryGetProperty("callsign", out JsonElement jsonPreviousCallsign))
                            {
                                Previous.Callsign = new Callsign(jsonPreviousCallsign.GetString().ToUpperInvariant());
                            }

                            if (jsonPrevious.TryGetProperty("operClass", out JsonElement jsonPreviousOperClass))
                            {
                                string previousOperClass = jsonPreviousOperClass.GetString().ToUpperInvariant();
                                Previous.OperClass = validOperatorClasses.Any(s => previousOperClass.Equals(s)) ? previousOperClass : null;
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
                            Address = new USAddress();

                            if (jsonAddress.TryGetProperty("attn", out JsonElement jsonAddressAttn))
                            {
                                Address.Attn = jsonAddressAttn.GetString();
                            }

                            if (jsonAddress.TryGetProperty("line1", out JsonElement jsonAddressLine1))
                            {
                                Address.Line1 = jsonAddressLine1.GetString();
                            }

                            if (jsonAddress.TryGetProperty("line2", out JsonElement jsonAddressLine2))
                            {
                                Address.Line2 = jsonAddressLine2.GetString();
                            }
                        }

                        JsonElement location = root.GetProperty("location");

                        Location = new USLocation()
                        {
                            Latitude = location.GetProperty("latitude").GetDecimal(),
                            Longitude = location.GetProperty("longitude").GetDecimal(),
                            GridSquare = location.GetProperty("gridsquare").GetString(),
                        };

                        JsonElement otherInfo = root.GetProperty("otherInfo");

                        OtherInfo = new USOtherInfo()
                        {
                            GrantDate = otherInfo.GetProperty("grantDate").GetDateTimeOffset(),
                            ExpiryDate = otherInfo.GetProperty("expiryDate").GetDateTimeOffset(),
                            LastActionDate = otherInfo.GetProperty("lastActionDate").GetDateTimeOffset(),
                            FRN = otherInfo.GetProperty("frn").GetString(),
                            UlsUri = new Uri(otherInfo.GetProperty("ulsUrl").GetString()),
                        };

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

    public enum USLicenseType
    {
        Person,
        Club,
        Recreation,
        Races,
        Military,
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

    public class USAddress
    {
        public string Line1;
        public string Line2;
        public string Attn;
    }

    public class USLocation
    {
        public decimal Latitude;
        public decimal Longitude;
        public string GridSquare;
    }

    public class USOtherInfo
    {
        public DateTimeOffset GrantDate;
        public DateTimeOffset ExpiryDate;
        public DateTimeOffset LastActionDate;
        public string FRN;
        public Uri UlsUri;
    }
}
