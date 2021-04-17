using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Hammer.Core.Maps;

namespace Hammer.Core.Models
{
    /// <summary>
    /// Represents a License for a Person.
    /// </summary>
    public class PersonalLicense : BaseLicense, INotifyPropertyChanged
    {
        private OperatorClasses operatorClass;
        private IList<(Callsign, OperatorClasses)> historical;

        /// <summary>
        /// Represents a license's class. Only applies to licenses issued to people.
        /// </summary>
        public OperatorClasses OperatorClass
        {
            get => operatorClass;
            set
            {
                operatorClass = value;
                RaisePropertyChanged(nameof(OperatorClass));
            }
        }

        public IList<(Callsign, OperatorClasses)> Historical
        {
            get => historical;
            private set
            {
                historical = value;
                RaisePropertyChanged(nameof(Historical));
            }
        }

        /// <summary>
        /// Instantiates a License with a new random Guid and empty objects.
        /// </summary>
        public PersonalLicense()
        {
            Historical = new List<(Callsign, OperatorClasses)>();
        }

        public static PersonalLicense GetTestLicense()
        {
            return new PersonalLicense()
            {
                Status = LicenseStatus.ETESTLICENSEDATA,
                Callsign = new Callsign("X1B34"),
                Name = "Johnny Appleseed",
                Country = "US",
                AddressLine1 = "4321 Street Ave",
                AddressLine2 = "Anytown, OR 97000",
                GrantDate = new DateTimeOffset(new DateTime(2020, 1, 1), new TimeSpan(-4, 0, 0)),
                ModifiedDate = new DateTimeOffset(new DateTime(2020, 6, 15), new TimeSpan(-4, 0, 0)),
                ExpiryDate = new DateTimeOffset(new DateTime(2030, 1, 1), new TimeSpan(-4, 0, 0)),
                FRN = "0123456789",
                GridSquare = "FF99ff",
                ID = new Guid(),
                LicenseeType = LicenseeTypes.Person,
                OperatorClass = OperatorClasses.Technician,
                Location = new GeographicPoint(45, -123),
                UlsUri = new Uri("https://example.com/"),
            };
        }
    }
}
