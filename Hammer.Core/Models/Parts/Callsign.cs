using System;
using Hammer.Core.Callsigns;
using Hammer.Core.Helpers;

namespace Hammer.Core.Models
{
    public class Callsign
    {
        private string sign;
        private string region;

        public string Sign
        {
            get => sign;

            private set
            {
                sign = string.IsNullOrWhiteSpace(value)
                    ? throw new ArgumentException($"{nameof(Sign)} cannot be null or whitespace.", nameof(Sign))
                    : value;
            }
        }

        public string Region
        {
            get => region;
            set => region = value;
        }

        public Callsign(string sign)
        {
            // Check and sanitize the callsign input.
            Sign = Sanitizers.SanitizeCallsign(sign);

            // Set the callsign's region, if any.
            if (Issuers.TryGetRegion(Sign, out string _region))
            {
                Region = _region;
            }
        }

        public override string ToString() => Sign;

        public static explicit operator string(Callsign c) => c == null ? "" : c.Sign;
    }
}
