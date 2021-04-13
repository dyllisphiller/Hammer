using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Hammer.Core.Maps;

namespace Hammer.Core.Models
{
    public class License
    {
        private string country;

        /// <summary>
        /// A GUID identifying the license.
        /// </summary>
        /// <remarks>
        /// While each callsign is unique, it can have multiple license records (e.g., transferred licenses).
        /// </remarks>
        public Guid ID { get; set; }

        /// <summary>
        /// The callsign this license represents.
        /// </summary>
        public Callsign Callsign { get; set; }

        /// <summary>
        /// The licensee's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The status of the license, as returned by the API. Non-standard.
        /// </summary>
        public LicenseStatus Status { get; set; }

        /// <summary>
        /// Represents a license's licensee type, like PERSON or CLUB.
        /// </summary>
        public string LicenseType { get; set; }

        /// <summary>
        /// Represents a license's class. Only applies to licenses issued to people.
        /// </summary>
        public string OperatorClass { get; set; }

        /// <summary>
        /// Represents a license's FCC Registration Number.
        /// </summary>
        public string FRN { get; set; }

        /// <summary>
        /// Represents a license's Universal Licensing System URL.
        /// </summary>
        public Uri UlsUri { get; set; }

        /// <summary>
        /// Represents a license's date of issuance.
        /// </summary>
        public DateTimeOffset GrantDate { get; set; }

        /// <summary>
        /// Represents a license's date of expiration.
        /// </summary>
        public DateTimeOffset ExpiryDate { get; set; }

        /// <summary>
        /// Represents the date a license was last changed.
        /// </summary>
        public DateTimeOffset ModifiedDate { get; set; }

        /// <summary>
        /// Represents the location of the licensee.
        /// </summary>
        /// <seealso cref="GeographicPoint"/>
        public GeographicPoint Location { get; set; }

        /// <summary>
        /// Represents the grid square of the licensee.
        /// </summary>
        public string GridSquare { get; set; }

        /// <summary>
        /// Represents an organization licensee's trustee.
        /// </summary>
        public Callsign Trustee { get; set; }

        /// <summary>
        /// A street address like 123 E Main St.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// A city, state/province, and postal code, like New York, NY 10001.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// An attention line, if any.
        /// </summary>
        public string AddressAttn { get; set; }

        /// <summary>
        /// Represents a license's originating country/region using the ISO two-letter standard.
        /// This defaults to the user's current culture settings. en-US yields US, it-IT yields IT, and so on.
        /// </summary>
        public string Country
        {
            get => country;
            set
            {
                if (value != null && value.Length < 5 && value.Length > 1)
                {
                    country = value.ToUpperInvariant();
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(Country), $"{nameof(Country)} must be exactly two letters.");
                }
            }
        }

        [JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }

        /// <summary>
        /// Instantiates a License with a new random Guid and empty objects.
        /// </summary>
        public License()
        {
            ID = Guid.NewGuid();

            GrantDate = new DateTimeOffset();
            ExpiryDate = new DateTimeOffset();
            ModifiedDate = new DateTimeOffset();
            Location = new GeographicPoint();

            Country = "US";
        }

        public static void GetLicenseByCallsign(string json, out License newLicense)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException($"'{nameof(json)}' cannot be null or whitespace.", nameof(json));
            }

            newLicense = new License();
        }

        // TODO: Adapt this to use an, er, adapter so that other search providers can be dropped in.
        //public static void TryParse(System.IO.Stream json, out License license)
        //{
        //    License _license;
        //    License _trustee;

        //    if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json), "A non-null JSON string must be passed into this parameter.");

        //    var options = new JsonSerializerOptions
        //    {
        //        AllowTrailingCommas = true
        //    };

        //    JsonSerializer.DeserializeAsync<License>(json, options);

        //    license = new License();
        //}
        public static void TryParse(JObject json, out License license)
        {
            License _license;
            License _trustee;
            GeographicPoint _geographicPoint;
            DateTimeOffset _grantDate = new DateTimeOffset();
            DateTimeOffset _expiryDate = new DateTimeOffset();
            DateTimeOffset _lastActionDate = new DateTimeOffset();

            if (json != null)
            {

                if (!string.IsNullOrEmpty((string)json["trustee"]["callsign"]))
                {
                    _trustee = new License
                    {
                        Callsign = new Callsign((string)json["trustee"]["callsign"]),
                        Name = (string)json["trustee"]["name"]
                    };
                }
                else
                {
                    _trustee = null;
                }

                _geographicPoint = new GeographicPoint(
                    (double)json["location"]["latitude"],
                    (double)json["location"]["longitude"]
                );

                if (!string.IsNullOrEmpty((string)json["otherInfo"]["grantDate"]))
                {
                    _grantDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["grantDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty((string)json["otherInfo"]["expiryDate"]))
                {
                    _expiryDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["expiryDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty((string)json["otherInfo"]["lastActionDate"]))
                {
                    _lastActionDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["lastActionDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                _license = new License
                {
                    //Status = (string)json["status"],
                    LicenseType = (string)json["type"],
                    Name = (string)json["name"],
                    Callsign = new Callsign((string)json["current"]["callsign"]),
                    OperatorClass = (string)json["current"]["operClass"],
                    FRN = (string)json["frn"],
                    UlsUri = (Uri)json["otherInfo"]["ulsUrl"],

                    Trustee = new Callsign(_trustee.Callsign.ToString()),

                    AddressLine1 = (string)json["address"]["line1"],
                    AddressLine2 = (string)json["address"]["line2"],
                    AddressAttn = (string)json["address"]["attn"],

                    Location = _geographicPoint,

                    GridSquare = (string)json["location"]["gridsquare"],

                    GrantDate = _grantDate,
                    ExpiryDate = _expiryDate,
                    ModifiedDate = _lastActionDate
                };
            }
            else
            {
                throw new ArgumentNullException(nameof(json), $"{nameof(TryParse)} must have one JObject argument");
            }

            license = _license;
        }
    }

    public class LicenseViewModel
    public enum LicenseStatus
    {
        private readonly License defaultLicense = new License();
        public License DefaultLicense { get => defaultLicense; }
        Unknown,
        Valid,
        Invalid,
        Updating,
        ESIGNNOTUS,
        EDEFAULTVIEWMODEL,
    }
}
