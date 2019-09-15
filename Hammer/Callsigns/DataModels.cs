using Hammer.Callsigns;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Hammer.Models
{
    /// <summary>
    /// Represents a point on Earth in degrees latitude and longitude.
    /// </summary>
    public class GeographicPoint
    {
        private double latitude;
        private double longitude;

        /// <summary>
        /// The latitude in degrees, between -90.0 and 90.0 (inclusive).
        /// </summary>
        public double Latitude
        {
            get => latitude;
            set
            {
                if (value >= -90.0 && value <= 90.0)
                {
                    latitude = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(Latitude)} must be between -90.0 and 90.0.");
                }
            }
        }

        /// <summary>
        /// The longitude in degrees, between -180.0 and 180.0 (inclusive).
        /// </summary>
        public double Longitude
        {
            get => longitude;
            set
            {
                if (value >= -180.0 && value <= 180.0)
                {
                    longitude = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(Longitude)} must be between -180.0 and 180.0.");
                }
            }
        }
        public string Coordinates
        {
            get
            {
                return $"{Latitude}, {Longitude}";
            }
        }

        /// <summary>
        /// Initializes a new instance of the Hammer.Models.GeographicPoint class with coordinates of (0.0, 0.0).
        /// </summary>
        public GeographicPoint()
        {
            Latitude = 0.0;
            Longitude = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of the Hammer.Models.GeographicPoint class for the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">Latitude in degrees from -90.0 to 90.0 (inclusive).</param>
        /// <param name="longitude">Longitude in degrees from -180.0 to 180.0 (inclusive).</param>
        public GeographicPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

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
        public DateTimeOffset ExpiryDate { get => expiryDate; set => lastActionDate = value; }
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

        //public TryParse()
        //{
        //    Location = new GeographicPoint();
        //}

        //public void TryParse(string callsign, string name, string status, string type, string operatorClass, string frn, string ulsUri, DateTimeOffset grantDate, DateTimeOffset expiryDate, DateTimeOffset lastActionDate)

        public void TryParse(JObject json)
        {
            if (json != null)
            {
                Status = (string)json["status"];
                Type = (string)json["type"];
                Callsign = (string)json["current"]["callsign"];
                OperatorClass = (string)json["current"]["operClass"];
                FRN = (string)json["frn"];
                UlsUri = (Uri)json["otherInfo"]["ulsUri"];

                if (!String.IsNullOrEmpty((string)json["trustee"]["callsign"]))
                {
                    License Trustee = new License();
                    Trustee.Callsign = (string)json["trustee"]["callsign"];
                    Trustee.Name = (string)json["trustee"]["name"];
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
            else
            {
                throw new ArgumentNullException(nameof(json), $"{nameof(TryParse)} must have one JObject argument");
            }
        }
    }

    public class LicenseViewModel
    {
        private License defaultLicense = new License();
        public License DefaultLicense { get => defaultLicense; }
    }
}