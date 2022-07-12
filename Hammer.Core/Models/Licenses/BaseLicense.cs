using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hammer.Core.Helpers;

namespace Hammer.Core.Models
{
    public abstract class BaseLicense : INotifyPropertyChanged
    {
        private Guid id;
        private Callsign callsign;
        private Callsign previousCallsign;
        private string name;
        private LicenseStatus status;
        private LicenseeTypes licenseeType;
        private string frn;
        private Uri ulsUri;
        private DateTimeOffset grantDate;
        private DateTimeOffset expiryDate;
        private DateTimeOffset modifiedDate;
        private GeographicPoint location;
        private string gridSquare;
        private string addressLine1;
        private string addressLine2;
        private string addressAttn;
        private string country;

        /// <summary>
        /// A GUID identifying the license.
        /// </summary>
        public Guid ID
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// The callsign this license represents.
        /// </summary>
        public Callsign Callsign
        {
            get => callsign;
            set
            {
                callsign = value;
                RaisePropertyChanged();
            }
        }

        public Callsign PreviousCallsign
        {
            get => previousCallsign;
            set
            {
                previousCallsign = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The licensee's name.
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The status of the license.
        /// </summary>
        public LicenseStatus Status
        {
            get => status;
            set
            {
                status = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Represents a license's licensee type, like Person or Club.
        /// </summary>
        public LicenseeTypes LicenseeType
        {
            get => licenseeType;
            set => licenseeType = value;
        }

        /// <summary>
        /// Represents a licensee's FCC Registration Number.
        /// </summary>
        public string FRN
        {
            get => frn;
            set
            {
                frn = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Represents a license's Universal Licensing System URL.
        /// </summary>
        public Uri UlsUri
        {
            get => ulsUri;
            set
            {
                ulsUri = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Represents a license's date of issuance.
        /// </summary>
        public DateTimeOffset GrantDate
        {
            get => grantDate;
            set
            {
                grantDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Represents a license's date of expiration.
        /// </summary>
        public DateTimeOffset ExpiryDate
        {
            get => expiryDate;
            set
            {
                expiryDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Represents the date a license was last changed.
        /// </summary>
        public DateTimeOffset ModifiedDate
        {
            get => modifiedDate;
            set
            {
                modifiedDate = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Represents the location of the licensee.
        /// </summary>
        /// <seealso cref="GeographicPoint"/>
        public GeographicPoint Location
        {
            get => location;
            set
            {
                location = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Represents the grid square of the licensee.
        /// </summary>
        public string GridSquare
        {
            get => gridSquare;
            set
            {
                gridSquare = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// A street address like 123 E Main St.
        /// </summary>
        public string AddressLine1
        {
            get => addressLine1;
            set
            {
                addressLine1 = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// A city, state/province, and postal code, like New York, NY 10001.
        /// </summary>
        public string AddressLine2
        {
            get => addressLine2;
            set
            {
                addressLine2 = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// An attention line, if any.
        /// </summary>
        public string AddressAttn
        {
            get => addressAttn;
            set
            {
                addressAttn = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Represents a license's originating country/region using the ISO two-letter standard.
        /// </summary>
        public virtual string Country
        {
            get => country;
            set
            {
                country = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseLicense()
        {
            ID = Guid.NewGuid();
            GrantDate = new DateTimeOffset();
            ExpiryDate = new DateTimeOffset();
            ModifiedDate = new DateTimeOffset();
            Location = new GeographicPoint();
        }
    }
}
