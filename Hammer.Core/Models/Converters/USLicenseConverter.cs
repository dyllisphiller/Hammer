using Hammer.Core.Models.Regional;

namespace Hammer.Core.Models.Converters
{
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
                FRN = usLicense.OtherInfo.Frn,
                GrantDate = usLicense.OtherInfo.GrantDate,
                Location = new Maps.GeographicPoint(usLicense.Location.Latitude, usLicense.Location.Longitude),
                GridSquare = usLicense.Location.GridSquare,
                ModifiedDate = usLicense.OtherInfo.LastActionDate,
                LicenseType = usLicense.LicenseType,
                Name = usLicense.Name,
                OperatorClass = usLicense.Current.OperClass.ToString(),
                Trustee = usLicense.Trustee,
            };

            return license;
        }
    }
}
