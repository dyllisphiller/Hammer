using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hammer.Core.Models.Converters.US
{
    public class USLicense
    {
        public USStatus Status { get; set; }
        [JsonPropertyName("type")]
        public USLicenseType LicenseType { get; set; }
        public USLicenseData Current { get; set; }
        public USLicenseData Previous { get; set; }
        public USTrusteeData Trustee { get; set; }
        public string Name { get; set; }
        public USAddress Address { get; set; }
        public USLocation Location { get; set; }
        public USOtherInfo OtherInfo { get; set; }
        [JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }

    public static partial class ConverterExtensions
    {
        public static License ToLicense(this USLicense usLicense)
        {
            License license = new License()
            {
                AddressAttn = usLicense.Address.Attn,
                AddressLine1 = usLicense.Address.Line1,
                AddressLine2 = usLicense.Address.Line2,
                Callsign = usLicense.Current.Callsign,
                Country = "us",
                ExpiryDate = usLicense.OtherInfo.ExpiryDate,
                FRN = usLicense.OtherInfo.FRN,
                GrantDate = usLicense.OtherInfo.GrantDate,
                GridSquare = usLicense.Location.GridSquare,
                LastActionDate = usLicense.OtherInfo.LastActionDate,
                LicenseType = usLicense.LicenseType.ToString(),
                Name = usLicense.Name,
                OperatorClass = usLicense.Current.OperClass.ToString(),
                Trustee = new License()
                {
                    Callsign = usLicense.Trustee.Callsign,
                    Name = usLicense.Trustee.Name,
                },
                UlsUri = usLicense.OtherInfo.UlsUri,
            };

            return license;
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
        Novice,
        Technician,
        TechnicianPlus,
        General,
        Advanced,
        Extra,
    }

    public struct USLicenseData
    {
        public Callsign Callsign;
        public USOperatorClass OperClass;
    }

    public struct USTrusteeData
    {
        public Callsign Callsign;
        public string Name;
    }

    public struct USAddress
    {
        public string Line1;
        public string Line2;
        public string Attn;
    }

    public struct USLocation
    {
        public decimal Latitude;
        public decimal Longitude;
        public string GridSquare;
    }

    public struct USOtherInfo
    {
        public DateTimeOffset GrantDate;
        public DateTimeOffset ExpiryDate;
        public DateTimeOffset LastActionDate;
        public string FRN;
        public Uri UlsUri;
    }
}
