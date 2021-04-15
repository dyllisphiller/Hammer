using Hammer.Core.Models.Regional;

namespace Hammer.Core.Models.Converters
{
    public static partial class ConverterExtensions
    {
        public static License ToLicense(this USLicense usLicense)
        {
            License license = new License()
            {
                Status = USLicenseStatusConverter(usLicense.Status),
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
                LicenseeType = usLicense.LicenseType,
                Name = usLicense.Name,
                OperatorClass = usLicense.Current.OperClass.ToString(),
                Trustee = usLicense.Trustee,
            };

            return license;
        }

        private static LicenseStatus USLicenseStatusConverter(string usLicenseStatus)
        {
            usLicenseStatus = usLicenseStatus.ToUpperInvariant();
            LicenseStatus licenseStatus;
            switch (usLicenseStatus)
            {
                case "VALID":
                    licenseStatus = LicenseStatus.Valid;
                    break;
                case "UPDATING":
                    licenseStatus = LicenseStatus.Updating;
                    break;
                case "INVALID":
                    licenseStatus = LicenseStatus.Invalid;
                    break;
                default:
                    licenseStatus = LicenseStatus.Unknown;
                    break;
            }

            return licenseStatus;
        }
    }
}
