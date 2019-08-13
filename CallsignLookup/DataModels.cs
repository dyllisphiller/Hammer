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
        public class CallsignModel
        {
            string _callsign;
            public string Callsign
            {
                get
                {
                    return _callsign;
                }
                set
                {
                    Regex _rgx = new Regex(@"[^a-zA-Z0-9]");

                    if (String.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentNullException();
                    }
                    else if (_rgx.IsMatch(value))
                    {
                        throw new ApplicationException("That is an invalid callsign.");
                    }
                    else
                    {
                        _callsign = value;
                    }
                }
            }
        }
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
            public string FRN {
                get;
                set;
            }
            public Uri UlsUrl {
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
        public Models.AddressModel Address {
            get;
            set;
        }
        public Models.LocationModel Location {
            get;
            set;
        }
        public Models.DatesModel Dates {
            get;
            set;
        }

        public LicenseModel(JObject jsonLicenseData)
        {
            Meta = new Models.MetaModel();
            Address = new Models.AddressModel();
            Location = new Models.LocationModel();
            Dates = new Models.DatesModel();

            Meta.Status = (string)jsonLicenseData["status"];
            Meta.Type = (string)jsonLicenseData["type"];
            Meta.Callsign = (string)jsonLicenseData["current"]["callsign"];
            Meta.FRN = (string)jsonLicenseData["frn"];
            Meta.UlsUrl = (Uri)jsonLicenseData["otherInfo"]["ulsUri"];
        }
    }
}
