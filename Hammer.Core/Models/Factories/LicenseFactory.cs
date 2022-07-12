using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Hammer.Core.Helpers;

namespace Hammer.Core.Models
{
    public static class Factories
    {
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

        public static BaseLicense MakeLicense(string jsonString)
        {
            BaseLicense license;

            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;

                string statusString = root.GetProperty("status").GetString();
                LicenseStatus _status = Parsers.ParseEnum<LicenseStatus>(statusString);

                if (_status == LicenseStatus.Valid)
                {
                    string licenseeTypeString = root.GetProperty("type").GetString().ToUpperInvariant();
                    LicenseeTypes licenseeType = Parsers.ParseEnum<LicenseeTypes>(licenseeTypeString);

                    license = NewLicenseByType(licenseeType);

                    if (root.TryGetProperty("current", out JsonElement jsonCurrent))
                    {
                        string callsignString = jsonCurrent.GetProperty("callsign").GetString().ToUpperInvariant();
                        license.Callsign = new Callsign(callsignString);

                        if (license is PersonalLicense _pl
                            && jsonCurrent.TryGetProperty("operClass", out JsonElement jsonCurrentOperClass))
                        {
                            string currentOperClass = jsonCurrentOperClass.GetString().ToUpperInvariant();
                            _pl.OperatorClass = Parsers.ParseEnum<OperatorClasses>(currentOperClass);
                        }
                    }

                    if (root.TryGetProperty("previous", out JsonElement jsonPrevious))
                    {
                        if (jsonCurrent.TryGetProperty("callsign", out JsonElement jsonPreviousCallsign))
                        {
                            string previousCallsign = jsonPreviousCallsign.GetString().ToUpperInvariant();
                            license.PreviousCallsign = new Callsign(previousCallsign);
                        }

                        if (license is PersonalLicense _pl
                            && jsonPrevious.TryGetProperty("operClass", out JsonElement jsonPreviousOperClass))
                        {
                            string previousOperClass = jsonPreviousOperClass.GetString();
                            if (!string.IsNullOrWhiteSpace(previousOperClass))
                            {
                                _pl.PreviousOperatorClass = Parsers.ParseEnum<OperatorClasses>(previousOperClass);
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

                        return license;
                    }
                }
                else
                {
                    return new PersonalLicense();
                }
            }
            return new PersonalLicense();
        }
    }
}
