using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CallsignLookup
{
    public class Models
    {
        public class AddressModel
        {
            public string Line1 {
                get;
                set;
            }
            public string Line2 {
                get;
                set;
            }
            public string Attn {
                get;
                set;
            }
        }
        public class LocationModel
        {
            public double Latitude {
                get;
                set;
            }
            public double Longitude {
                get;
                set;
            }
            public string GridSquare {
                get;
                set;
            }
        }
        public class DatesModel
        {
            public DateTimeOffset Grant {
                get;
                set;
            }
            public DateTimeOffset Expiry {
                get;
                set;
            }
            public DateTimeOffset LastAction {
                get;
                set;
            }
        }
        public class MetaModel
        {
            public string Callsign {
                get;
                set;
            }
            public string Status {
                get;
                set;
            }
            public string Type
            {
                get;
                set;
            }
            public string OperatorClass
            {
                get;
                set;
            }
            public string FRN {
                get;
                set;
            }
            public Uri UlsUrl {
                get;
                set;
            }
        }
        public class TrusteeModel
        {
            public string Callsign
            {
                get;
                set;
            }
            public string Name
            {
                get;
                set;
            }
        }
    }
    public class LicenseModel
    {
        public Models.MetaModel Meta {
            get;
            set;
        }
        public Models.TrusteeModel Trustee
        {
            get;
            set;
        }
        public Models.AddressModel Address
        {
            get;
            set;
        }
        public Models.LocationModel Location
        {
            get;
            set;
        }
        public Models.DatesModel Dates
        {
            get;
            set;
        }

        public void TryParse(JObject json)
        {
            Meta = new Models.MetaModel();
            Trustee = new Models.TrusteeModel();
            Address = new Models.AddressModel();
            Location = new Models.LocationModel();
            Dates = new Models.DatesModel();

            Meta.Status = (string)json["status"];
            Meta.Type = (string)json["type"];
            Meta.Callsign = (string)json["current"]["callsign"];
            Meta.OperatorClass = (string)json["current"]["operClass"];
            Meta.FRN = (string)json["frn"];
            Meta.UlsUrl = (Uri)json["otherInfo"]["ulsUri"];

            Trustee.Callsign = (string)json["trustee"]["callsign"];
            Trustee.Name = (string)json["trustee"]["name"];

            Address.Line1 = (string)json["address"]["line1"];
            Address.Line2 = (string)json["address"]["line2"];
            Address.Attn = (string)json["address"]["attn"];

            Location.Latitude = (double)json["location"]["latitude"];
            Location.Longitude = (double)json["location"]["longitude"];
            Location.GridSquare = (string)json["location"]["gridsquare"];

            // Wrangle dates
            Dates.Grant = DateTimeOffset.Parse(
                (string)json["otherInfo"]["grantDate"],
                System.Globalization.CultureInfo.InvariantCulture);
            Dates.Expiry = DateTimeOffset.Parse(
                (string)json["otherInfo"]["expiryDate"],
                System.Globalization.CultureInfo.InvariantCulture);
            Dates.LastAction = DateTimeOffset.Parse(
                (string)json["otherInfo"]["lastActionDate"],
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
