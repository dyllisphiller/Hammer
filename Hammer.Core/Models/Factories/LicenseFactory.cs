using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Hammer.Core.Maps;

namespace Hammer.Core.Models
{
    public class Factories
    {
        private static OperatorClasses ParseOperatorClass(string operatorClassString)
        {
            return !Enum.TryParse(operatorClassString, true, out OperatorClasses operatorClass)
                ? operatorClass
                : OperatorClasses.Unknown;
        }

        private static LicenseeTypes ParseLicenseeType(string licenseeTypeString)
        {
            return !Enum.TryParse(licenseeTypeString, true, out LicenseeTypes licenseeType)
                ? licenseeType
                : LicenseeTypes.Unknown;
        }

        private static LicenseStatus ParseLicenseStatus(string licenseStatusString)
        {
            return !Enum.TryParse(licenseStatusString, true, out LicenseStatus licenseStatus)
                ? licenseStatus
                : LicenseStatus.Unknown;
        }

        private static BaseLicense NewLicenseByType(LicenseeTypes licenseeType)
        {
            switch (licenseeType)
            {
                case LicenseeTypes.Person:
                    return new PersonalLicense();
                case LicenseeTypes.Club:
                    return new ClubLicense();
                default:
                    return null;
            }
        }

        public BaseLicense MakeLicense(string jsonString)
        {
            BaseLicense license;

            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;

                LicenseStatus _status = ParseLicenseStatus(root.GetProperty("status").GetString());

                if (_status == LicenseStatus.Valid)
                {
                    string licenseeTypeString = root.GetProperty("type").GetString().ToUpperInvariant();
                    LicenseeTypes licenseeType = ParseLicenseeType(licenseeTypeString);
                    license = NewLicenseByType(licenseeType);

                    JsonElement current = root.GetProperty("current");


                    if (root.TryGetProperty("current", out JsonElement jsonCurrent))
                    {
                        license.Callsign = new Callsign(current.GetProperty("callsign").GetString().ToUpperInvariant());

                        if (license is PersonalLicense _pl
                            && jsonCurrent.TryGetProperty("operClass", out JsonElement jsonCurrentOperClass))
                        {
                            string currentOperClass = jsonCurrentOperClass.GetString().ToUpperInvariant();
                            _pl.OperatorClass = ParseOperatorClass(currentOperClass);
                        }
                    }

                    // If there is a previous callsign and/or operator class, add Previous data
                    if (license is PersonalLicense _pl)
                    {
                        if (root.TryGetProperty("previous", out JsonElement jsonPrevious))
                        {
                            if (jsonPrevious.TryGetProperty("callsign", out JsonElement jsonPreviousCallsign)
                                && jsonPrevious.TryGetProperty("operClass", out JsonElement jsonPreviousOperClass))
                            {
                                string previousCallsign = jsonPreviousCallsign.GetString();
                                string previousOperClass = jsonPreviousOperClass.GetString();

                                if (!string.IsNullOrWhiteSpace(previousCallsign)
                                    && !string.IsNullOrWhiteSpace(previousOperClass))
                                {
                                    _pl.Historical.Add((
                                        new Callsign(previousCallsign),
                                        ParseOperatorClass(previousOperClass)));
                                }
                            }
                        }
                    }

                    if (license is ClubLicense _cl)
                    {
                        if (root.TryGetProperty("trustee", out JsonElement jsonTrustee))
                        {
                            if (jsonTrustee.TryGetProperty("callsign", out JsonElement jsonTrusteeCallsign) && !string.IsNullOrWhiteSpace(jsonTrusteeCallsign.GetString()))
                            {
                                _cl.Trustee = new Trustee() { Callsign = new Callsign(jsonTrusteeCallsign.GetString()) };
                            }
                        }
                    }

                    if (root.TryGetProperty("name", out JsonElement jsonName)
                        && !string.IsNullOrEmpty(jsonName.GetString()))
                    {
                        license.Name = jsonName.GetString();
                    }

                    if (root.TryGetProperty("address", out JsonElement jsonAddress))
                    {
                        license.AddressAttn = jsonAddress.TryGetProperty("attn", out JsonElement jsonAddressAttn) ? jsonAddressAttn.GetString() : "";
                        license.AddressLine1 = jsonAddress.TryGetProperty("line1", out JsonElement jsonAddressLine1) ? jsonAddressLine1.GetString() : "";
                        license.AddressLine2 = jsonAddress.TryGetProperty("line2", out JsonElement jsonAddressLine2) ? jsonAddressLine2.GetString() : "";
                    }

                    if (root.TryGetProperty("location", out JsonElement jsonLocation))
                    {
                        license.Location = new GeographicPoint
                        {
                            Latitude = jsonLocation.TryGetProperty("latitude", out JsonElement jsonLocationLatitude)
                                ? Convert.ToDouble(jsonLocationLatitude.GetString(), CultureInfo.GetCultureInfo("en-US"))
                                : 0,
                            Longitude = jsonLocation.TryGetProperty("longitude", out JsonElement jsonLocationLongitude)
                                ? Convert.ToDouble(jsonLocationLongitude.GetString(), CultureInfo.GetCultureInfo("en-US"))
                                : 0,
                        };
                        license.GridSquare = jsonLocation.TryGetProperty("gridsquare", out JsonElement jsonLocationGridSquare) ? jsonLocationGridSquare.GetString() : "";
                    }

                    if (root.TryGetProperty("otherInfo", out JsonElement jsonOtherInfo))
                    {
                        if (jsonOtherInfo.TryGetProperty("grantDate", out JsonElement jsonGrantDate))
                        {
                            license.GrantDate = DateTimeOffset.TryParse(jsonGrantDate.GetString(), out DateTimeOffset _date)
                                                ? _date
                                                : new DateTimeOffset();
                        }

                        if (jsonOtherInfo.TryGetProperty("expiryDate", out JsonElement jsonExpiryDate))
                        {
                            license.ExpiryDate = DateTimeOffset.TryParse(jsonExpiryDate.GetString(), out DateTimeOffset _date)
                                                 ? _date
                                                 : new DateTimeOffset();
                        }

                        if (jsonOtherInfo.TryGetProperty("lastActionDate", out JsonElement jsonLastActionDate))
                        {
                            license.ModifiedDate = DateTimeOffset.TryParse(jsonLastActionDate.GetString(), out DateTimeOffset _date)
                                                   ? _date
                                                   : new DateTimeOffset();
                        }

                        if (jsonOtherInfo.TryGetProperty("frn", out JsonElement jsonFrn))
                        {
                            license.FRN = jsonFrn.GetString();
                        }

                        if (jsonOtherInfo.TryGetProperty("ulsUrl", out JsonElement jsonUlsUrl))
                        {
                            license.UlsUri = new Uri(jsonUlsUrl.GetString());
                        }
                    }
                }
            }
            return license;
        }
    }
}
