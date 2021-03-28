using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hammer.Core.Globalization
{
    public static partial class Regions
    {
        public static RegionInfo GetRegionInfo(string lang, string region)
        {
            if (region.Length != 2) throw new ArgumentOutOfRangeException($"{nameof(region)} must be exactly two characters.", nameof(region));
            if (lang.Length != 2) throw new ArgumentOutOfRangeException($"{nameof(lang)} must be exactly two characters.", nameof(lang));
            return new RegionInfo(lang + "-" + region);
        }
    }
}
