namespace Hammer.Core.Models
{
    public enum LicenseStatus
    {
        /// <summary>
        /// Indicates the license status is unknown. Default.
        /// </summary>
        Unknown,
        /// <summary>
        /// Indicates the callsign and license are valid.
        /// </summary>
        Valid,
        /// <summary>
        /// Indicates the callsign or license is invalid.
        /// </summary>
        Invalid,
        /// <summary>
        /// Indicates the API source is updating.
        /// </summary>
        Updating,
    }
}
