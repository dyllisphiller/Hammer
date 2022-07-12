using System;
using System.Collections.Generic;
using System.Text;

namespace Hammer.Core.Models
{
    public class TestLicense : BaseLicense
    {
        public TestLicense()
        {
            Status = LicenseStatus.Unknown;
            Callsign = new Callsign("X1B34");
            Name = "Johnny Appleseed";
            Country = "US";
            AddressLine1 = "IF YOU ARE SEEING THIS";
            AddressLine2 = "SOMETHING IS TERRIBLY WRONG";
            GrantDate = new DateTimeOffset(new DateTime(2010, 1, 1), new TimeSpan(-4, 0, 0));
            ModifiedDate = new DateTimeOffset(new DateTime(2010, 6, 15), new TimeSpan(-4, 0, 0));
            ExpiryDate = new DateTimeOffset(new DateTime(2020, 1, 1), new TimeSpan(-4, 0, 0));
            FRN = "0123456789";
            GridSquare = "FF99ff";
            ID = new Guid();
            LicenseeType = LicenseeTypes.Unknown;
            Location = new GeographicPoint(45, -123);
            UlsUri = new Uri("https://example.com/");
        }
    }
}
