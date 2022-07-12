/*using Hammer.Core.Models.Regional;

namespace Hammer.Core.Models.Converters
{
    public static partial class LicenseExtensions
    {
        public static BaseLicense ToLicense(this USLicense usLicense)
        {
            PersonalLicense license = new PersonalLicense()
            {
                Status = usLicense.Status,
                Country = "US",
                AddressAttn = usLicense.AddressAttn,
                AddressLine1 = usLicense.AddressLine1,
                AddressLine2 = usLicense.AddressLine2,
                Callsign = usLicense.Current.Callsign,
                ExpiryDate = usLicense.ExpiryDate,
                FRN = usLicense.Frn,
                GrantDate = usLicense.GrantDate,
                Location = new Maps.GeographicPoint(usLicense.Location.Latitude, usLicense.Location.Longitude),
                GridSquare = usLicense.GridSquare,
                ModifiedDate = usLicense.LastActionDate,
                LicenseeType = usLicense.LicenseeType,
                Name = usLicense.Name,
                OperatorClass = usLicense.Current.OperClass,
            };

            return license;
        }
    }
}
*/