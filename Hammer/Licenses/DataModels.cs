using Hammer.Callsigns;
using Hammer.Core.Cartography;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace Hammer.Licenses
{
    

    public class License
    {
        private string callsign;
        private string name;
        private string status;
        private string type;
        private string operatorClass;
        private string frn;
        private Uri ulsUri;
        private DateTimeOffset grantDate = new DateTimeOffset();
        private DateTimeOffset expiryDate = new DateTimeOffset();
        private DateTimeOffset lastActionDate = new DateTimeOffset();
        private GeographicPoint location = new GeographicPoint();
        private string gridSquare;
        private License trustee;

        public string Callsign { get => callsign; set => callsign = value; }
        public string Name { get => name; set => name = value; }
        public string Status { get => status; set => status = value; }
        public string Type { get => type; set => type = value; }
        public string OperatorClass { get => operatorClass; set => operatorClass = value; }
        public string FRN { get => frn; set => frn = value; }
        public Uri UlsUri { get => ulsUri; set => ulsUri = value; }
        public DateTimeOffset GrantDate { get => grantDate; set => grantDate = value; }
        public DateTimeOffset ExpiryDate { get => expiryDate; set => expiryDate = value; }
        public DateTimeOffset LastActionDate { get => lastActionDate; set => lastActionDate = value; }
        public GeographicPoint Location { get => location; set => location = value; }
        public string GridSquare { get => gridSquare; set => gridSquare = value; }
        public License Trustee { get => trustee; set => trustee = value; }

        /// <summary>
        /// A street address, such as 123 E Main St.
        /// </summary>
        public string AddressLine1 { get; set; }
        /// <summary>
        /// A city, state, and ZIP code, like New York, NY 10001.
        /// </summary>
        public string AddressLine2 { get; set; }
        /// <summary>
        /// An attention line, if any, minus the Attn: prefix.
        /// </summary>
        public string AddressAttn { get; set; }
        /// <summary>
        /// The country from which the callsign originates.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Tries to parse a JObject into the properties of the License instance. This method must be called from a class instance.
        /// </summary>
        /// <param name="json">The JObject to parse.</param>
        /// <returns>true if successful; otherwise, false</returns>
        public bool TryParse(JObject json)
        {
            if (json == null)
            {
                return false;
            }
            else
            {
                Status = (string)json["status"];
                Type = (string)json["type"];
                Name = (string)json["name"];
                Callsign = (string)json["current"]["callsign"];
                OperatorClass = (string)json["current"]["operClass"];
                FRN = (string)json["frn"];
                UlsUri = (Uri)json["otherInfo"]["ulsUrl"];

                if (!string.IsNullOrEmpty((string)json["trustee"]["callsign"]))
                {
                    License Trustee = new License
                    {
                        Callsign = (string)json["trustee"]["callsign"],
                        Name = (string)json["trustee"]["name"]
                    };
                }

                AddressLine1 = (string)json["address"]["line1"];
                AddressLine2 = (string)json["address"]["line2"];
                AddressAttn = (string)json["address"]["attn"];

                Location.Latitude = (double)json["location"]["latitude"];
                Location.Longitude = (double)json["location"]["longitude"];

                GridSquare = (string)json["location"]["gridsquare"];

                // Wrangle dates
                if (!String.IsNullOrEmpty((string)json["otherInfo"]["grantDate"]))
                {
                    GrantDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["grantDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!String.IsNullOrEmpty((string)json["otherInfo"]["expiryDate"]))
                {
                    ExpiryDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["expiryDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                if (!String.IsNullOrEmpty((string)json["otherInfo"]["lastActionDate"]))
                {
                    LastActionDate = DateTimeOffset.Parse(
                        (string)json["otherInfo"]["lastActionDate"],
                        System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return true;
        }
    }

    public class LicenseViewModel
    {
        private readonly License defaultLicense = new License();
        public License DefaultLicense { get => defaultLicense; }
    }
}
