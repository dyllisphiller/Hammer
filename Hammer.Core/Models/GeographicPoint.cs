using System;

namespace Hammer.Core.Cartography
{
    /// <summary>
    /// Represents a point on Earth in degrees latitude and longitude.
    /// </summary>
    /// <remarks>
    /// This is similar to Windows.Devices.Geolocation.GeoPoint but simplified and platform-agnostic.
    /// </remarks>
    public class GeographicPoint
    {
        private double latitude;
        private double longitude;

        /// <summary>
        /// The latitude in degrees, between -90.0 and 90.0 (inclusive).
        /// </summary>
        public double Latitude
        {
            get => latitude;
            set
            {
                if (value >= -90.0 && value <= 90.0)
                {
                    latitude = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(Latitude)} must be between -90.0 and 90.0.");
                }
            }
        }

        public double Longitude
        {
            get => longitude;
            set
            {
                if (value >= -180.0 && value <= 180.0)
                {
                    longitude = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(Longitude)} must be between -180.0 and 180.0.");
                }
            }
        }
        public override string ToString()
        {
            return $"{Latitude}, {Longitude}";
        }

        /// <summary>
        /// Initializes a new GeographicPoint with the default coordinates (0.0, 0.0).
        /// </summary>
        public GeographicPoint()
        {
            Latitude = 0.0;
            Longitude = 0.0;
        }

        /// <summary>
        /// Initializes a new GeographicPoint with the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">Latitude in degrees from -90.0 to 90.0 (inclusive).</param>
        /// <param name="longitude">Longitude in degrees from -180.0 to 180.0 (inclusive).</param>
        public GeographicPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
