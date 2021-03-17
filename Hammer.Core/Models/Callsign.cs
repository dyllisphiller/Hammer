using System;
using System.Collections.Generic;
using System.Text;

namespace Hammer.Core.Models
{
    public class Callsign
    {
        public string Sign { get; private set; }
        public Callsign()
        {

        }

        public Callsign(string sign)
        {
            Sign = sign;
        }
    }
}
