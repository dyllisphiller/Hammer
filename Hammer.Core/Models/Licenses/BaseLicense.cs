using System;
using System.Collections.Generic;
using Hammer.Core.Maps;

namespace Hammer.Core.Models
{
    public abstract class BaseLicense
    {
        /// <summary>
        /// A GUID identifying the license.
        /// </summary>
        /// <remarks>
        /// While each callsign is unique, it can have multiple license records (e.g., transferred licenses).
        /// </remarks>
        public Guid ID { get; set; }

        /// <summary>
        /// The callsign this license represents.
        /// </summary>
        public Callsign Callsign { get; set; }

        /// <summary>
        /// The licensee's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The status of the license.
        /// </summary>
        public LicenseStatus Status { get; set; }

        /// <summary>
        /// Represents a license's licensee type, like Person or Club.
        /// </summary>
        public LicenseeTypes LicenseeType { get; set; }

        /// <summary>
        /// Represents a licensee's FCC Registration Number.
        /// </summary>
        public string FRN { get; set; }

        /// <summary>
        /// Represents a license's Universal Licensing System URL.
        /// </summary>
        public Uri UlsUri { get; set; }

        /// <summary>
        /// Represents a license's date of issuance.
        /// </summary>
        public DateTimeOffset GrantDate { get; set; }

        /// <summary>
        /// Represents a license's date of expiration.
        /// </summary>
        public DateTimeOffset ExpiryDate { get; set; }

        /// <summary>
        /// Represents the date a license was last changed.
        /// </summary>
        public DateTimeOffset ModifiedDate { get; set; }

        /// <summary>
        /// Represents the location of the licensee.
        /// </summary>
        /// <seealso cref="GeographicPoint"/>
        public GeographicPoint Location { get; set; }

        /// <summary>
        /// Represents the grid square of the licensee.
        /// </summary>
        public string GridSquare { get; set; }

        /// <summary>
        /// A street address like 123 E Main St.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// A city, state/province, and postal code, like New York, NY 10001.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// An attention line, if any.
        /// </summary>
        public string AddressAttn { get; set; }

        /// <summary>
        /// Represents a license's originating country/region using the ISO two-letter standard.
        /// </summary>
        public virtual string Country { get; set; }
    }
}
