using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
