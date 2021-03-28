using System;
using System.Collections.Generic;
using System.Text;

namespace Hammer.Core.Models
{
    public class Callsign
    {
        private string sign;

        public string Sign
        {
            get => sign;

            private set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{nameof(Sign)} cannot be null or whitespace.", nameof(Sign));
                sign = value;
            }
        }

        public Callsign(string sign)
        {
            Sign = sign;
        }

        public override string ToString()
        {
            return Sign;
        }

        public static explicit operator string(Callsign c)
        {
            return c.Sign;
        }
    }
}
