using Hammer.Core.Cartography;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hammer.Core.Models
{
    /// <summary>
    /// Encapsulates arbitrary key/value fields for non-universal licensee data.
    /// </summary>
    /// <remarks>
    /// This is a shortcut to support those fields exclusive to a certain licensing authority.
    /// </remarks>
    public class LicenseField
    {
        private string fieldType;
        private string fieldKey;
        private string fieldValue;

        /// <summary>
        /// The type of the field; string, int, double, and so on.
        /// </summary>
        public string Type { get => fieldType; set => fieldType = value; }

        /// <summary>
        /// The name of the field.
        /// </summary>
        public string Key { get => fieldKey; set => fieldKey = value; }

        /// <summary>
        /// The value of the field.
        /// </summary>
        public string Value { get => fieldValue; set => fieldValue = value; }
    }

    public class License
    {
        private string callsign;
        private string status;
        private string type;
        private string operatorClass;
        private string frn;
        private Uri ulsUri;
        private DateTimeOffset grantDate;
        private DateTimeOffset expiryDate;
        private DateTimeOffset lastActionDate;
        private GeographicPoint location;
        private string gridSquare;
        private License trustee;
        private string addressLine1;
        private string addressLine2;
        private string addressAttn;
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
        public string Callsign { get => callsign; set => callsign = value; }

        /// <summary>
        /// A list of arbitrary LicenseField-encapsulated data points.
        /// </summary>
        public IList<LicenseField> LicenseFields { get; }

        /// <summary>
        /// Represents a licensee's name.
        /// </summary>
        /// <value>Gets/sets the value of the string field <c>name</c>.</value>
        public string Name { get; set; }

        /// <summary>
        /// Represents the status returned by the API.
        /// </summary>
        /// <value>Gets/sets the value of the string field <c>status</c>.</value>
        public string Status
        {
            get => status;
            set
            {
                if (value != null)
                {
                    if (value == "VALID" || value == "INVALID" || value == "UPDATING")
                    {
                        status = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} must be VALID, INVALID, or UPDATING.");
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(value), $"{nameof(status)} cannot be null.");
                }
            }
        }

        /// <summary>
        /// Represents a license's licensee type, like PERSON or CLUB.
        /// </summary>
        public string Type { get => type; set => type = value; }

        /// <summary>
        /// Represents a license's class. Only applies to licenses issued to people.
        /// </summary>
        public string OperatorClass { get => operatorClass; set => operatorClass = value; }

        /// <summary>
        /// Represents a license's FCC Registration Number.
        /// </summary>
        public string FRN { get => frn; set => frn = value; }

        /// <summary>
        /// Represents a license's Universal Licensing System URL.
        /// </summary>
        public Uri UlsUri { get => ulsUri; set => ulsUri = value; }

        /// <summary>
        /// Represents a license's date of issuance.
        /// </summary>
        public DateTimeOffset GrantDate { get => grantDate; set => grantDate = value; }

        /// <summary>
        /// Represents a license's date of expiration.
        /// </summary>
        public DateTimeOffset ExpiryDate { get => expiryDate; set => expiryDate = value; }

        /// <summary>
        /// Represents the date a license was last changed.
        /// </summary>
        public DateTimeOffset LastActionDate { get => lastActionDate; set => lastActionDate = value; }

        /// <summary>
        /// Represents the location of the licensee.
        /// </summary>
        /// <seealso cref="Hammer.Core.Cartography.GeographicPoint"/>
        public GeographicPoint Location { get => location; set => location = value; }

        /// <summary>
        /// Represents the grid square of the licensee.
        /// </summary>
        public string GridSquare { get => gridSquare; set => gridSquare = value; }

        /// <summary>
        /// Represents an organization licensee's trustee.
        /// </summary>
        public License Trustee { get => trustee; set => trustee = value; }

        /// <summary>
        /// A street address like 123 E Main St.
        /// </summary>
        public string AddressLine1 { get => addressLine1; set => addressLine1 = value; }

        /// <summary>
        /// A city, state/province, and postal code, like New York, NY 10001.
        /// </summary>
        public string AddressLine2 { get => addressLine2; set => addressLine2 = value; }

        /// <summary>
        /// An attention line, if any.
        /// </summary>
        public string AddressAttn { get => addressAttn; set => addressAttn = value; }

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
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(Country)} must be exactly two letters.");
                }
            }
        }

        /// <summary>
        /// Instantiates a License with a new random Guid and empty objects.
        /// </summary>
        public License()
        {
            ID = Guid.NewGuid();

            GrantDate = new DateTimeOffset();
            ExpiryDate = new DateTimeOffset();
            LastActionDate = new DateTimeOffset();
            Location = new GeographicPoint();

            Country = System.Globalization.CultureInfo.CurrentCulture.Name.Substring(3, 2);
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

                if (!String.IsNullOrEmpty((string)json["trustee"]["callsign"]))
                {
                    _trustee = new License
                    {
                        Callsign = (string)json["trustee"]["callsign"],
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

                if (!String.IsNullOrEmpty((string)json["otherInfo"]["grantDate"]))
                {
                    _grantDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["grantDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!String.IsNullOrEmpty((string)json["otherInfo"]["expiryDate"]))
                {
                    _expiryDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["expiryDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!String.IsNullOrEmpty((string)json["otherInfo"]["lastActionDate"]))
                {
                    _lastActionDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["lastActionDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                _license = new License
                {
                    Status = (string)json["status"],
                    Type = (string)json["type"],
                    Name = (string)json["name"],
                    Callsign = (string)json["current"]["callsign"],
                    OperatorClass = (string)json["current"]["operClass"],
                    FRN = (string)json["frn"],
                    UlsUri = (Uri)json["otherInfo"]["ulsUrl"],

                    Trustee = _trustee,

                    AddressLine1 = (string)json["address"]["line1"],
                    AddressLine2 = (string)json["address"]["line2"],
                    AddressAttn = (string)json["address"]["attn"],

                    Location = _geographicPoint,

                    GridSquare = (string)json["location"]["gridsquare"],

                    GrantDate = _grantDate,
                    ExpiryDate = _expiryDate,
                    LastActionDate = _lastActionDate
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
    {
        private readonly License defaultLicense = new License();
        public License DefaultLicense { get => defaultLicense; }
    }
}
